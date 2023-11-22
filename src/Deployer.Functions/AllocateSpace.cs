using System;
using System.Threading.Tasks;
using ByteSizeLib;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem.Utils;
using Serilog;
using Zafiro.Core.Pending;
using Zafiro.Storage;

namespace Deployer.Functions
{
    public class AllocateSpace : DeployerFunction
    {
        private readonly IStorage storage;

        public AllocateSpace(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task Execute(string partitionDescriptor, string size)
        {
            var requiredSize = ByteSize.TryParse(size, out var b)
                ? Result.Success(b)
                : Result.Failure<ByteSize>($"Could not parse {size} to a valid byte size");
            var partition = await storage.GetPartitionFromDescriptor(partitionDescriptor);

            await requiredSize.WithBind(partition, (bs, part) => Allocate(part, bs));
        }

        private async Task<Result> Allocate(IPartition partition, ByteSize requiredSpace)
        {
            var volume = await partition.GetVolume();

            var data = volume.Size;
            var allocated = partition.Disk.AllocatedSize;
            var available = partition.Disk.AvailableSize;
            var newSize = data - (requiredSpace - available);

            if (DifferenceIsNotRelevant(available, requiredSpace))
            {
                Log.Information(
                    "The difference between requested size {RequiredSpace} and available size {Available} wasn't big enough to do a resize operation",
                    requiredSpace, available);
                return Result.Success();
            }

            Log.Verbose("Total size allocated: {Size}", allocated);
            Log.Verbose("Space available: {Size}", available);
            Log.Verbose("Space needed: {Size}", requiredSpace);
            Log.Verbose("Current size: {Size}", data);
            Log.Verbose("Calculated new size for partition: {Size}", newSize);

            Log.Verbose("Resizing 'Data' to {Size}", newSize);

            await volume.Partition.Resize(newSize);

            Log.Information("Resize operation finished");

            var refreshedDish = await storage.GetDisk((int)partition.Disk.Number);
            var errorMessage = string.Format("The attempt to resize the partition failed. Unable to allocate {0} in {1}", requiredSpace.ToString(),
                partition);
            return refreshedDish.Ensure(d => d.HasEnoughSpace(requiredSpace), errorMessage);
        }

        private static bool DifferenceIsNotRelevant(ByteSize available, ByteSize requiredSpace)
        {
            var difference = requiredSpace - available;
            var tolerance = ByteSize.FromMegaBytes(50);
            return Math.Abs(difference.Bytes) <= tolerance.Bytes;
        }
    }
}