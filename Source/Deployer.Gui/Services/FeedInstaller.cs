using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Gui.ViewModels;
using Serilog;

namespace Deployer.Gui.Services
{
    public class FeedInstaller : IFeedInstaller
    {
        private readonly IDeployer deployer;
        private readonly IFileSystem fileSystem;

        public FeedInstaller(IDeployer deployer, IFileSystem fileSystem)
        {
            this.deployer = deployer;
            this.fileSystem = fileSystem;
        }

        public async Task<Result> Install()
        {
            var deploymentFeedPath = Constants.GetDeploymentFeedPath(fileSystem);
            Log.Information("Downloading Deployment Feed to {Path}", deploymentFeedPath);
            var initialState = new Dictionary<string, object> { ["downloadFolder"] = deploymentFeedPath};
            var run = await deployer.Run("Bootstrap.txt", initialState);
            var result = run.Match(summary => Result.Success(), e => Result.Failure("Failed to download the Deployment Feed"));
            Log.Information("Deployment feed downloaded successfully");

            //throw new InvalidOperationException();

            return result;
        }
    }
}