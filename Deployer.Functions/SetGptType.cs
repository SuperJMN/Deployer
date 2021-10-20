using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class SetGptType : DeployerFunction
    {
        private readonly IStorage storage;

        public SetGptType(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task Execute(string partitionDescriptor, string gptTypeString)
        {
            var partition = await storage.GetPartitionFromDescriptor(partitionDescriptor);
            await partition.Tap(p => p.SetGptType(GptType.FromString(gptTypeString)));
        }
    }
}