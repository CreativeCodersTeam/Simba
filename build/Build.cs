using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;

[GitHubActions("integration", GitHubActionsImage.UbuntuLatest,
    OnPushBranches = ["feature/**"],
    OnPullRequestBranches = ["main"],
    InvokedTargets = ["clean", "restore", "build", "publish"],
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
)]
[GitHubActions("main", GitHubActionsImage.UbuntuLatest,
    OnPushBranches = ["main"],
    InvokedTargets = ["clean", "restore", "build", "publish"],
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
)]
[GitHubActions(ReleaseWorkflow, GitHubActionsImage.UbuntuLatest,
    OnPushTags = ["v**"],
    InvokedTargets = ["clean", "restore", "build", "publish", "CreateDistPackages", "CreateGithubRelease"],
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
)]
class Build : NukeBuild,
    IGitRepositoryParameter,
    IConfigurationParameter,
    IGitVersionParameter,
    ISourceDirectoryParameter,
    IArtifactsSettings,
    ICleanTarget, IBuildTarget, IRestoreTarget, IPublishTarget, ICreateDistPackagesTarget, ICreateGithubReleaseTarget
{
    const string ReleaseWorkflow = "release";
    
    public static int Main () => Execute<Build>(x => ((IBuildTarget)x).Build);

    public Build()
    {
        FileSys.Directory.CreateDirectory(DistOutputPath);
    }
    
    [Parameter(Name = "GITHUB_TOKEN")] string GitHubToken;
    
    Target CreateLinuxArchive => _ => _
        .DependsOn<IPublishTarget>()
        .Produces(GetDistDir() / "simbasrv.tar.gz")
        .Executes<Task>(async () =>
        {
            Process
                .Start("tar",
                [
                    "-czf",
                    GetDistDir() / "simbasrv.tar.gz",
                    "-C",
                    GetDistDir() / "simbasrv",
                    "."
                ])
                .WaitForExit();

            await CreateGitHubRelease(GetDistDir() / "simbasrv.tar.gz")
                .ConfigureAwait(false);
        });

    private async Task CreateGitHubRelease(AbsolutePath archiveFileName)
    {
        if (GitHubActions.Instance?.IsPullRequest != false)
        {
            return;
        }
        
        GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue("CreativeCoders.Nuke"))
        {
            Credentials = new Credentials(GitHubToken)
        };

        var release = await GitHubTasks.GitHubClient.Repository.Release
            .Create("CreativeCodersTeam", "Simba",
                new NewRelease("0.1.2")
                {
                    Name = "Release 0.1.2",
                    Body = "New release 0.1.2",
                    Draft = true,
                    Prerelease = !string.IsNullOrWhiteSpace(((IGitVersionParameter) this).GitVersion?.PreReleaseTag)
                })
            .ConfigureAwait(false);
        
        var assetContentType = "application/x-gtar";
        
        var releaseAssetUpload = new ReleaseAssetUpload
        {
            ContentType = assetContentType,
            FileName = FileSys.Path.GetFileName(archiveFileName),
            RawData = FileSys.File.OpenRead(archiveFileName)
        };
        _ = await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(release, releaseAssetUpload);
        
        await GitHubTasks.GitHubClient.Repository.Release
            .Edit("CreativeCodersTeam", "Simba", release.Id, new ReleaseUpdate { Draft = false });
    }

    IEnumerable<PublishingItem> IPublishSettings.PublishingItems =>
    [
        new PublishingItem(
            GetSourceDir() / "CreativeCoders.Simba.Server.Linux" / "CreativeCoders.Simba.Server.Linux.csproj",
            GetDistDir() / "simbasrv")
    ];

    string GetVersion() => ((IGitVersionParameter) this).GitVersion?.NuGetVersionV2 ?? "0.1-unknown";

    AbsolutePath GetSourceDir() => ((ISourceDirectoryParameter) this).SourceDirectory;

    AbsolutePath GetDistDir() => ((IArtifactsSettings) this).ArtifactsDirectory / "dist";

    public IEnumerable<DistPackage> DistPackages =>
    [
        new DistPackage($"simbasrv-{GetVersion()}", GetDistDir() / "simbasrv") { Format = DistPackageFormat.TarGz }
    ];

    public AbsolutePath DistOutputPath => GetDistDir() / "packages";

    public string ReleaseName => $"Release {GetVersion()}";
    
    public string ReleaseBody => $"Release {GetVersion()}";

    public string ReleaseVersion => GetVersion();

    public IEnumerable<GithubReleaseAsset> ReleaseAssets =>
    [
        new GithubReleaseAsset(DistOutputPath / $"simbasrv-{GetVersion()}.tar.gz")
    ];
}
