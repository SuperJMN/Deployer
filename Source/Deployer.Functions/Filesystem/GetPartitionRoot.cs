using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Zafiro.Storage;

namespace Deployer.Functions.Filesystem
{
    public class GetPartitionRoot : DeployerFunction
    {
        private readonly IStorage storage;

        public GetPartitionRoot(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Result<string>> Execute(string partitionDescriptor)
        {
            var part = await storage.GetPartitionFromDescriptor(partitionDescriptor);
            return await part.Map(async p =>
            {
                await p.EnsureWritable();
                return p.Root.Remove(p.Root.Length - 1);
            });
        }
    }
}