using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class RemovePartitionSafe : DeployerFunction
    {
        private readonly IStorage storage;

        public RemovePartitionSafe(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task Execute(string partitionDescriptor)
        {
            var partition = await storage.GetPartitionFromDescriptor(partitionDescriptor);
            await partition.Tap(p => p.Remove());
            await partition.Tap(p => p.Disk.Refresh());
        }
    }
}