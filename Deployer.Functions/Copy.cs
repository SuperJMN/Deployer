using System.IO.Abstractions;
using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.Storage.Misc;

namespace Deployer.Functions
{
    public class Copy : DeployerFunction
    {
        private IFileSystem fileSystem;

        public Copy(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public Task Execute(string origin, string destination)
        {
            return fileSystem.Copy(origin, destination);
        }
    }
}