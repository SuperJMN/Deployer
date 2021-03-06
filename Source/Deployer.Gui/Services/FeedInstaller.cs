using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reflection;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
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
            var bootstrapPath = fileSystem.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log.Information("Installing Deployment Feed to {Path}", deploymentFeedPath);
            var initialState = new Dictionary<string, object> { ["downloadFolder"] = deploymentFeedPath};
            var run = await deployer.Run(fileSystem.Path.Combine(bootstrapPath, "Bootstrap.txt"), initialState);
            var result = run.Match(_ =>
            {
                Log.Information("Feed installed successfully");
                return Result.Success();
            }, e =>
            {
                Log.Error("Feed failed to install. Reason: {e}", e.ToString());
                return Result.Failure("Failed to download the Deployment Feed");
            });

            return result;
        }
    }
}