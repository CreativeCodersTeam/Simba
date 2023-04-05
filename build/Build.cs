using System.Collections.Generic;
using System.Diagnostics;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;

[GitHubActions("integration", GitHubActionsImage.UbuntuLatest,
    OnPushBranches = new[]{"feature/**"},
    OnPullRequestBranches = new[]{"main"},
    InvokedTargets = new []{"clean", "restore", "compile", "publish", "CreateLinuxArchive"},
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
)]
[GitHubActions("main", GitHubActionsImage.UbuntuLatest,
    OnPushBranches = new[]{"main"},
    InvokedTargets = new []{"clean", "restore", "compile", "publish"},
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
)]
[GitHubActions(ReleaseWorkflow, GitHubActionsImage.UbuntuLatest,
    OnPushTags = new []{"v**"},
    InvokedTargets = new []{"clean", "restore", "compile", "publish"},
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
    ICleanTarget, ICompileTarget, IRestoreTarget, IPublishTarget
{
    const string ReleaseWorkflow = "release";
    
    public static int Main () => Execute<Build>(x => ((ICompileTarget)x).Compile);

    Target CreateLinuxArchive => _ => _
        .DependsOn<IPublishTarget>()
        .Produces(GetDistDir() / "simbasrv.tar.gz")
        .Executes(() =>
        {
            Process
                .Start("tar", new[]
                {
                    "-czf",
                    GetDistDir() / "simbasrv.tar.gz",
                    GetDistDir() / "simbasrv"
                })
                .WaitForExit();
        });
    
    IEnumerable<PublishingItem> IPublishSettings.PublishingItems => new[]
    {
        new PublishingItem(
            GetSourceDir() / "CreativeCoders.Simba.Server.Linux" / "CreativeCoders.Simba.Server.Linux.csproj",
            GetDistDir() / "simbasrv")
    };

    //string GetVersion() => ((IGitVersionParameter) this).GitVersion?.NuGetVersionV2 ?? "0.1-unknown";

    AbsolutePath GetSourceDir() => ((ISourceDirectoryParameter) this).SourceDirectory;

    AbsolutePath GetDistDir() => ((IArtifactsSettings) this).ArtifactsDirectory / "dist";
}
