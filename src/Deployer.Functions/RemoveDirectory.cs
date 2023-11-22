using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Deployer.Functions.Core;
using Serilog;

namespace Deployer.Functions
{
    public class RemoveDirectory : DeployerFunction
    {
        private readonly IFileSystem fileSystem;

        public RemoveDirectory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public async Task Execute(string path)
        {
            try
            {
                fileSystem.Directory.Delete(path, true);
            }
            catch (Exception e)
            {
                Log.Information(e, $"Couldn't delete directory {path}");
            }
        }
    }
}