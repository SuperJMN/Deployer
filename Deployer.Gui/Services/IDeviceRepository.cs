using System.Collections.Generic;
using Deployer.Library;

namespace Deployer.Gui
{
    public interface IDeviceRepository
    {
        IEnumerable<Device> Get();
    }
}