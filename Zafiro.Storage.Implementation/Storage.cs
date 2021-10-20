using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using ByteSizeLib;
using CSharpFunctionalExtensions;

namespace Zafiro.Storage.Windows
{
    public class Storage : IStorage
    {
        public async Task<Result<IDisk>> GetDisk(int n)
        {
            var ps = PowerShell.Create();
            ps.AddScript($"Get-Disk {n}");
            var results = await ps.InvokeAsync();

            var disks = results
                .Select(x => x.ImmediateBaseObject)
                .Select(ToDisk);

            return disks.TryFirst().ToResult($"Couldn't find disk number {n}");
        }

        public async Task<IList<IDisk>> GetDisks()
        {
            var results = await PowerShellMixin.ExecuteScript("Get-Disk");

            var disks = results
                .Select(x => x.ImmediateBaseObject)
                .Select(ToDisk);

            return disks.ToList();
        }

        private IDisk ToDisk(object disk)
        {
            var number = (uint)disk.GetPropertyValue("Number");
            var size = new ByteSize((ulong)disk.GetPropertyValue("Size"));
            var allocatedSize = new ByteSize((ulong)disk.GetPropertyValue("AllocatedSize"));

            var diskInfo = new DiskInfo
            {
                Number = number,
                Size = size,
                AllocatedSize = allocatedSize,
                FriendlyName = (string)disk.GetPropertyValue("FriendlyName"),
                IsSystem = (bool)disk.GetPropertyValue("IsSystem"),
                IsBoot = (bool)disk.GetPropertyValue("IsBoot"),
                IsOffline = (bool)disk.GetPropertyValue("IsOffline"),
                IsReadOnly = (bool)disk.GetPropertyValue("IsReadOnly"),
                UniqueId = (string)disk.GetPropertyValue("UniqueId"),
                PartitionStyle = GetDiskType((ushort)disk.GetPropertyValue("PartitionStyle")),
            };

            return new Disk(diskInfo);
        }

        private DiskType GetDiskType(ushort index)
        {
            switch (index)
            {
                case 1:
                    return DiskType.Mbr;
                case 2:
                    return DiskType.Gpt;
                default:
                    return DiskType.Raw;
            }
        }
    }
}