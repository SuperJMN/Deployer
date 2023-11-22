using System.Collections.Generic;
using System.IO.Abstractions;
using Deployer.Library;

namespace Deployer.Gui.Services
{
    class DeviceRepository : IDeviceRepository
    {
        private readonly IDeployementSerializer deploymentSerializer;
        private readonly IFileSystem fileSystem;

        public DeviceRepository(IDeployementSerializer deploymentSerializer, IFileSystem fileSystem)
        {
            this.deploymentSerializer = deploymentSerializer;
            this.fileSystem = fileSystem;
        }

        public IEnumerable<Device> Get()
        {
            return deploymentSerializer
                .Deserialize(fileSystem.File.ReadAllText(fileSystem.Path.Combine(Constants.GetDeploymentFeedPath(fileSystem), "Deployments.xml")))
                .Devices;
        }
    }
}