using System.Collections.Generic;

namespace Deployer.Library
{
    public class DeployerStore 
    {
        public IEnumerable<Device> Devices { get; set; }
        public IEnumerable<Deployment> Deployments { get; set; }
    }
}