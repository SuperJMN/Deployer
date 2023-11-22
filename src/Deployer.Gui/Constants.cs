using System.IO.Abstractions;

namespace Deployer.Gui
{
    public class Constants
    {
        public static string GetDeploymentFeedPath(IFileSystem fileSystem) => fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), "Deployer", "Feed");
    }
}