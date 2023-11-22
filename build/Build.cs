using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Nuke.GitHub;
using Serilog;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.GitHub.GitHubTasks;
using static Nuke.Common.Tooling.ProcessTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.PackWindows);

    public AbsolutePath OutputDirectory = RootDirectory / "output";
    public AbsolutePath PublishDirectory => OutputDirectory / "publish";
    public AbsolutePath PackagesDirectory => OutputDirectory / "packages";

    [Solution] Solution Solution;
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    [GitVersion] readonly GitVersion GitVersion;
    [GitRepository] readonly GitRepository Repository;
    [Parameter("authtoken")] [Secret] readonly string GitHubAuthenticationToken;

    Target Clean => td => td
        .Executes(() =>
        {
            OutputDirectory.CreateOrCleanDirectory();
            var absolutePaths = RootDirectory.GlobDirectories("**/bin", "**/obj").Where(a => !((string)a).Contains("build")).ToList();
            Log.Information("Deleting {Dirs}", absolutePaths);
            absolutePaths.DeleteDirectories();
        });

    Target RestoreWorkloads => td => td
        .Executes(() =>
        {
            StartShell(@$"dotnet workload restore {Solution.Path}").AssertZeroExitCode();
        });

    Target PackDebian => td => td
        .DependsOn(Clean)
        .DependsOn(RestoreWorkloads)
        .Executes(() => DebPackages.Create(Solution, Configuration, PublishDirectory, PackagesDirectory, GitVersion.MajorMinorPatch));

    Target PackWindows => td => td
        .DependsOn(Clean)
        .DependsOn(RestoreWorkloads)
        .Executes(() =>
        {
            var desktopProject = Solution.AllProjects.First(project => project.Name.EndsWith("Desktop"));
            var runtimes = new[] { "win-x64", };

            DotNetPublish(settings => settings
                .SetConfiguration(Configuration)
                .SetProject(desktopProject)
                .CombineWith(runtimes, (c, runtime) =>
                    c.SetRuntime(runtime)
                        .SetOutput(PublishDirectory / runtime)));

            runtimes.ForEach(rt =>
            {
                var src = PublishDirectory / rt;
                var zipName = $"{Solution.Name}-{rt}.zip";
                var dest = PackagesDirectory / zipName;
                Log.Information("Zipping {Input} to {Output}", src, dest);
                src.ZipTo(dest);
            });
        });

    Target PackAndroid => td => td
        .DependsOn(Clean)
        .DependsOn(RestoreWorkloads)
        .Executes(() =>
        {
            var androidProject = Solution.AllProjects.First(project => project.Name.EndsWith("Android"));
        
            DotNetPublish(settings => settings
                .SetProperty("ApplicationVersion", GitVersion.CommitsSinceVersionSource)
                .SetProperty("ApplicationDisplayVersion", GitVersion.MajorMinorPatch)
                .SetConfiguration(Configuration)
                .SetProject(androidProject)
                .SetOutput(PackagesDirectory));
        });


    Target Publish => td => td
        //.DependsOn(PackDebian)
        .DependsOn(PackWindows)
        //.DependsOn(PackAndroid)
        ;

    Target PublishGitHubRelease => td => td
        .OnlyWhenStatic(() => GitVersion.BranchName.Equals("master") || GitVersion.BranchName.Equals("origin/master"))
        .DependsOn(Publish)
        .Requires(() => GitHubAuthenticationToken)
        .Executes(async () =>
        {
            var releaseTag = $"v{GitVersion.MajorMinorPatch}";

            var repositoryInfo = GetGitHubRepositoryInfo(Repository);

            Log.Information("Commit for the release: {GitVersionSha}", GitVersion.Sha);

            Log.Information("Getting list of files in {Path}", PackagesDirectory);
            var artifacts = PackagesDirectory.GetFiles().ToList();
            Log.Information("List of files obtained successfully");

            Assert.NotEmpty(artifacts,
                "Could not find any package to upload to the release");

            await PublishRelease(x => x
                .SetArtifactPaths(artifacts.Select(path => (string)path).ToArray())
                .SetCommitSha(GitVersion.Sha)
                .SetRepositoryName(repositoryInfo.repositoryName)
                .SetRepositoryOwner(repositoryInfo.gitHubOwner)
                .SetTag(releaseTag)
                .SetToken(GitHubAuthenticationToken)
            );
        });
}