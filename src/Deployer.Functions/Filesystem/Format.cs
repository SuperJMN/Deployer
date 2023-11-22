using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Zafiro.Storage;

namespace Deployer.Functions.Filesystem
{
    public class Format : DeployerFunction
    {
        private readonly IStorage storage;

        public Format(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Result<bool>> Execute(string partitionDescriptor, string fileSystemFormat, string label = null)
        {
            var part = await storage.GetPartitionFromDescriptor(partitionDescriptor);
            return await part.Map(async p =>
            {
                await p.EnsureWritable();
                var vol = await p.GetVolume();
                var systemFormat = FileSystemFormat.FromString(fileSystemFormat);
                await vol.Format(systemFormat, label);
                return true;
            });
        }
    }
}