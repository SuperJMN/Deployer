using System.Threading.Tasks;
using ByteSizeLib;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class CreateGptPartition : DeployerFunction
    {
        private readonly IStorage storage;

        public CreateGptPartition(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Result<string>> Execute(int diskNumber, string partitionType, string gptName = "", string sizeString = "*")
        {
            var disk = await storage.GetDisk(diskNumber);

            return await disk
                .Bind(d => GetSize(d, sizeString).Map(size => new { d, size }))
                .Map(d => d.d.CreateGptPartition(GptType.FromString(partitionType), gptName, d.size))
                .Map(p => p.GetDescriptor());
        }

        private static Result<ByteSize> GetSize(IDisk disk, string sizeString)
        {
            if (sizeString.Trim() == "*")
            {
                return disk.AvailableSize;
            }

            return sizeString.AsByteSize();
        }
    }
}