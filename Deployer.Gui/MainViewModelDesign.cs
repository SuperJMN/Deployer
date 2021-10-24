using System.Collections.Generic;
using System.Linq;
using Deployer.Gui.ViewModels;
using Deployer.Library.Generator;
using ReactiveUI;

namespace Deployer.Gui
{
    public class MainViewModelDesign : ViewModelBase
    {
        private DeviceViewModel selectedDevice;

        public MainViewModelDesign()
        {
            Devices = DefaultStore.Create()
                .Devices
                .Select(device => new DeviceViewModel(device, new NullDeployer()))
                .ToList();
        }

        public List<DeviceViewModel> Devices { get; }

        public DeviceViewModel SelectedDevice
        {
            get => selectedDevice;
            set => this.RaiseAndSetIfChanged(ref selectedDevice, value);
        }

        public string StatusMessage => "Saludos";

        public bool IsBusy => true;

        public OperationStatusViewModelDesign OperationStatus => new();
    }
}