using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DotnetPackaging.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;

class DebPackages
{
    public static async Task Create(Solution solution,
        Configuration configuration,
        AbsolutePath publishDirectory,
        AbsolutePath outputDirectory,
        string version)
    {
        var desktopProject = solution.AllProjects.First(project => project.Name.EndsWith("Desktop"));
        var runtimes = new[] { "linux-x64", "linux-arm64" };

        DotNetTasks.DotNetPublish(settings => settings
            .SetConfiguration(configuration)
            .SetProject(desktopProject)
            .CombineWith(runtimes, (c, runtime) =>
                c.SetRuntime(runtime)
                    .SetOutput(publishDirectory / runtime)));

        var results = new List<Result>();

        foreach (var runtime in runtimes)
        {
            string projectName = solution.Name;
            string architecture = runtime.Split("-")[1];

            string packageName = $"{projectName!.Replace(" ", "").ToLower()}_{version}_{architecture}.deb";

            var fromFile = await new FileInfo(solution.Directory / "metadata.deb.json").ToPackageDefinition();
            var packageDefinition = fromFile with
            {
                Metadata = fromFile.Metadata with
                {
                    Version = version,
                    Architecture = GetArchitecture(architecture)
                }
            };
                
            Log.Information("Creating {Package}", packageName);
            var result = await DotnetPackaging.Create.Deb(packageDefinition, publishDirectory / runtime, outputDirectory / packageName);
            result
                .Finally(r =>
                {
                    Log.Information("{Package} {Result}", packageName, r.Match(() => $"{packageName} created successfully!", error => $"{packageName} creation failed: {error}"));
                    return r;
                });

            results.Add(result);
        }
        
        if (results.Combine().IsFailure)
        {
            throw new Exception(".deb creation failed");
        }
    }

    static string GetArchitecture(string architecture)
    {
        return architecture switch
        {
            "x64" => "amd64",
            "arm64" => "arm64",
            _ => throw new NotSupportedException($"Invalid architecture {architecture}"),
        };
    }
}