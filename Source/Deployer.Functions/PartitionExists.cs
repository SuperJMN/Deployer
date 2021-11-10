using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class PartitionExists : DeployerFunction
    {
        private readonly IStorage storage;

        public PartitionExists(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<bool> Execute(string partitionDescriptor)
        {
            var part = await storage.GetPartitionFromDescriptor(partitionDescriptor);
            return part.Match(_ => true, _ => false);
        }
    }
}