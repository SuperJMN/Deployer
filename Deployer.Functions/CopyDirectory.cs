using System.IO.Abstractions;
using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.Storage.Misc;

namespace Deployer.Functions
{
    public class CopyDirectory : DeployerFunction
    {
        private readonly IFileSystem fileSystem;

        public CopyDirectory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public Task Execute(string origin, string destination)
        {
            return fileSystem.CopyDirectory(origin, destination);
        }
    }
}