using ByteSizeLib;

namespace Deployer.Gui.ViewModels.Design
{
    public class DiskViewModelDesign : IDiskViewModel
    {
        public uint Number { get; } = 1;
        public string FriendlyName { get; } = "Some disk";
        public ByteSize Size { get; } = ByteSize.FromGigaBytes(256);
        public bool IsUsualTarget { get; } = true;
    }
}