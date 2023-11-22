using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class SetPartitionLayout : DeployerFunction
    {
        private readonly IStorage storage;

        public SetPartitionLayout(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Result<bool>> Execute(int diskNumber, string diskType)
        {
            var type = DiskType.FromString(diskType);
            var disk = await storage.GetDisk(diskNumber);
            await disk.Tap(d => d.ClearAs(type));
            return true;
        }
    }
}