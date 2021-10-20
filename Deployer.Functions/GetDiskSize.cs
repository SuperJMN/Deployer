using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class GetDiskSize : DeployerFunction
    {
        private readonly IStorage storage;

        public GetDiskSize(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Result<double>> Execute(int diskNumber)
        {
            return (await storage.GetDisk(diskNumber)).Map(disk => disk.AvailableSize.Bytes);
        }
    }
}