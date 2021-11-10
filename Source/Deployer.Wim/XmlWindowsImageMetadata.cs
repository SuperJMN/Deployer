using System.Collections.Generic;

namespace Deployer.Wim
{
    public class XmlWindowsImageMetadata
    {
        public IList<DiskImageMetadata> Images { get; set; } = new List<DiskImageMetadata>();
    }
}