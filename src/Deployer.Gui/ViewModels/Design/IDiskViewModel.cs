using ByteSizeLib;

namespace Deployer.Gui.ViewModels.Design
{
    public interface IDiskViewModel
    {
        uint Number { get; }
        string FriendlyName { get; }
        ByteSize Size { get; }
        bool IsUsualTarget { get; }
    }
}