using ByteSizeLib;
using Deployer.Gui.ViewModels.Design;
using Zafiro.Storage;

namespace Deployer.Gui.ViewModels.Requirements
{
    public class DiskViewModel : IDiskViewModel
    {
        private readonly IDisk disk;

        public DiskViewModel(IDisk disk)
        {
            this.disk = disk;
        }

        public uint Number => disk.Number;
        public string FriendlyName => disk.FriendlyName;
        public ByteSize Size => disk.Size;
        public bool IsUsualTarget => Size > ByteSize.FromGigaBytes(1) && Size < ByteSize.FromGigaBytes(200);

        public override string ToString()
        {
            return $"Disk number {disk.Number}, {disk.FriendlyName}, Capacity: {disk.Size}";
        }
    }
}