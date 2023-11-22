namespace Deployer.Library.Generator
{
    public static class DefaultStore
    {
        private static readonly string Cityman = "https://github.com/WOA-Project/Deployment-Feed/blob/master/Assets/lumia950xl.png?raw=true";
        private static readonly string Talkman = "https://github.com/WOA-Project/Deployment-Feed/blob/master/Assets/lumia950.png?raw=true";
        private static string Rpi3 = "https://github.com/WOA-Project/Deployment-Feed/blob/master/Assets/rpi3.png?raw=true";
        private static string Rpi4 = "https://github.com/WOA-Project/Deployment-Feed/blob/master/Assets/rpi4.png?raw=true";

        private static readonly string Emmc =
            "https://github.com/WOA-Project/Deployment-Feed/blob/master/Assets/emmc.png?raw=true";

        private static string MicroSD = "https://github.com/WOA-Project/Deployment-Feed/blob/master/Assets/microsd.png?raw=true";

        public static DeployerStore Create()
        {
            var store = new DeployerStore();

            var cityman = new Device
            {
                Id = 1,
                Code = "Cityman",
                FriendlyName = "Lumia 950 XL",
                Icon = Cityman,
                Name = "Lumia 950 XL",
                Deployments = new[]
                {
                    new Deployment
                    {
                        Title = "Standard",
                        Description = "Deploys WOA into the phone's internal memory",
                        ScriptPath = "Devices\\Lumia\\950s\\Cityman\\Main.txt",
                        Icon = Emmc
                    }
                }
            };

            var talkman = new Device
            {
                Id = 2,
                Code = "Talkman",
                FriendlyName = "Lumia 950",
                Icon = Talkman,
                Name = "Lumia 950",
                Deployments = new[]
                {
                    new Deployment
                    {
                        Title = "Standard",
                        Description = "Deploys WOA into the phone's internal memory",
                        ScriptPath = "Devices\\Lumia\\950s\\Talkman\\Main.txt",
                        Icon = Emmc
                    }
                }
            };

            //var rpi3 = new Device
            //{
            //    Code = "RaspberryPi3",
            //    FriendlyName = "Raspberry Pi 3",
            //    Name = "Raspberry Pi 3",
            //    Icon = Rpi3,
            //};

            //var rpi4 = new Device
            //{
            //    Code = "RaspberryPi4",
            //    FriendlyName = "Raspberry Pi 4",
            //    Name = "Raspberry Pi 4",
            //    Icon = Rpi4,
            //};

            store.Devices = new[] { cityman, talkman };


            return store;
        }
    }
}