using System.Collections.Generic;

namespace Deployer.Library
{
    public class Deployment
    {
        public IEnumerable<Device> Devices { get; set; }
        public string ScriptPath { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
    }
}