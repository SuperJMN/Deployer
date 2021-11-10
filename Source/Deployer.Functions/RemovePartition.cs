using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class RemovePartition : DeployerFunction
    {
        private readonly IStorage storage;

        public RemovePartition(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Result> Execute(string partitionDescriptor)
        {
            var partition = await storage.GetPartitionFromDescriptor(partitionDescriptor);
            await partition.Tap(p => p.Remove());
            return await partition.Tap(p => p.Disk.Refresh());
        }
    }
}