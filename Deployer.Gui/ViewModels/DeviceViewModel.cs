using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        private readonly Device device;
        private DeploymentViewModel selectedDeployment;

        public DeviceViewModel(Device device, IDeployer deployer, IFileSystem fileSystem)
        {
            this.device = device;
            Deployments = device.Deployments.Select(deployment => new DeploymentViewModel(deployment, deployer, fileSystem)).ToList();
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