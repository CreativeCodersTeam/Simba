using System;
using System.Linq;
using CreativeCoders.NukeBuild;
using CreativeCoders.NukeBuild.BuildActions;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.GitHubActions.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Octokit;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[GitHubActions(
    "ci",
    GitHubActionsImage.UbuntuLatest,
    OnPushBranches = new []{"feature/**"},
    InvokedTargets = new[] { nameof(Restore) },
    FetchDepth = 0,
    EnableGitHubToken = true
)]
class Build : NukeBuild, IBuildInfo
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Restore);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    [GitRepository] readonly GitRepository GitRepository;

    [GitVersion] readonly GitVersion GitVersion;
    
    [CI] readonly GitHubActions GitHubActions;

    AbsolutePath SourceDirectory => RootDirectory / "source";

    AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";

    AbsolutePath TestBaseDirectory => RootDirectory / ".tests";

    AbsolutePath TestResultsDirectory => TestBaseDirectory / "results";

    AbsolutePath TestProjectsBasePath => SourceDirectory / "UnitTests";

    AbsolutePath CoverageDirectory => TestBaseDirectory / "coverage";

    AbsolutePath TempNukeDirectory => RootDirectory / ".nuke" / "temp";
    
    const string PackageProjectUrl = "https://github.com/CreativeCodersTeam/Simba";
    
    Target Clean => _ => _
        .Before(Restore)
        .UseBuildAction<CleanBuildAction>(this,
            x => x
                .AddDirectoryForClean(ArtifactsDirectory)
                .AddDirectoryForClean(TestBaseDirectory));

    Target Restore => _ => _
        .Executes(() =>
        {
            
            return DotNetTasks.DotNetRestore(s => s
                .SetProjectFile(Solution)
                //.SetSources("https://nuget.pkg.github.com/CreativeCodersTeam/index.json")
            );
        });
    
    Target Compile => _ => _
        .DependsOn(Restore)
        .UseBuildAction<DotNetCompileBuildAction>(this);

    string IBuildInfo.Configuration => Configuration;
    
    Solution IBuildInfo.Solution => Solution;
    
    GitRepository IBuildInfo.GitRepository => GitRepository;
    
    IVersionInfo IBuildInfo.VersionInfo => new GitVersionWrapper(GitVersion, "0.0.0", 1);
    
    AbsolutePath IBuildInfo.SourceDirectory => SourceDirectory;
    
    AbsolutePath IBuildInfo.ArtifactsDirectory => ArtifactsDirectory;
}
