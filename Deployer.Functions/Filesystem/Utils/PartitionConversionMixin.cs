using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Filesystem.MicroParser;
using Zafiro.Storage;

namespace Deployer.Functions.Filesystem.Utils
{
    public static class PartitionConversionMixin
    {
        public static Task<Result<IPartition>> GetPartitionFromDescriptor(this IStorage fileSystem, string descriptor)
        {
            return Parser.Parse(descriptor)
                .Bind(micro => GetPartition(fileSystem, micro, descriptor));
        }

        private static async Task<Result<IPartition>> GetPartition(IStorage fileSystem, Micro mini, string descriptor)
        {
            var diskNumber = (int?)mini["Disk"];

            if (diskNumber == null)
            {
                throw new InvalidOperationException($"Disk value is missing while parsing partition descriptor: {descriptor}");
            }

            var label = (string)mini["Label"];
            var name = (string)mini["Name"];
            var number = (int?)mini["Number"];

            var disk = await fileSystem.GetDisk(diskNumber.Value);
            var result = await disk.Map(async disk =>
            {
                IPartition part = null;

                if (number.HasValue)
                {
                    part = await disk.GetPartitionByNumber(number.Value);
                }

                if (part is null && !(label is null))
                {
                    part = await disk.GetPartitionByVolumeLabel(label);
                }

                if (part is null && name != null)
                {
                    part = await disk.GetPartitionByName(name);
                }

                if (part != null)
                {
                    return Result.Success(part);
                }

                return Result.Failure<IPartition>($"Could not retrieve partition from descriptor {descriptor}");
            });

            return result;
        }
    }
}