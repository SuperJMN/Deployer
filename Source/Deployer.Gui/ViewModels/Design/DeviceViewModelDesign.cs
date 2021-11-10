using System;

namespace Deployer.Gui.ViewModels.Design
{
    public class DeviceViewModelDesign
    {
        public DeploymentViewModelDesign SelectedDeployment { get; } = new();

        public string Icon => "https://github.com/WOA-Project/Deployment-Feed/blob/master/Assets/lumia950xl.png?raw=true";

        public string FriendlyName { get; } = "Lumia 950 XL";
    }
}