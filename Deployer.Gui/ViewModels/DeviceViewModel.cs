using System.Collections.Generic;
using System.Linq;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        private readonly Device device;
        private DeploymentViewModel selectedDeployment;

        public DeviceViewModel(Device device)
        {
            this.device = device;
            Deployments = device.Deployments.Select(deployment => new DeploymentViewModel(deployment)).ToList();
        }

        public DeploymentViewModel SelectedDeployment
        {
            get => selectedDeployment;
            set => this.RaiseAndSetIfChanged(ref selectedDeployment, value);
        }

        public string Icon => device.Icon;
        public string FriendlyName => device.FriendlyName;
        public string Code => device.Code;
        public string Variant => device.Variant;

        public List<DeploymentViewModel> Deployments { get; }
    }
}