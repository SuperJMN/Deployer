﻿using System.Collections.Generic;

namespace Deployer.Library
{
    public class Device
    {
        public string Code { get; set; }
        public string FriendlyName { get; set; }
        public string Name { get; set; }
        public string Variant { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public IEnumerable<Deployment> Deployments { get; set; }
    }
}
