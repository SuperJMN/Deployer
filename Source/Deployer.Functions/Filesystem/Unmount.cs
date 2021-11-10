using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Superpower.Model;
using Zafiro.Storage;
using Result = CSharpFunctionalExtensions.Result;

namespace Deployer.Functions.Filesystem
{
    public class Unmount : DeployerFunction
    {
        private readonly IStorage storage;

        public Unmount(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Result> Execute(string partitionDescriptor)
        {
            var part = await storage.GetPartitionFromDescriptor(partitionDescriptor);
            return await part.Bind(async partition =>
            {
                await partition.RemoveDriveLetter();
                return Result.Success(Unit.Value);
            });
        }
    }
}