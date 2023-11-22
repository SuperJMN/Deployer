using Zafiro.System.Windows;

namespace Deployer.Wim
{
    public class DiskImageMetadata
    {
        public int Index { get; set; }
        public string DisplayName { get; set; }
        public ProcessorArchitecture Architecture { get; set; }
        public string Build { get; set; }
    }
}