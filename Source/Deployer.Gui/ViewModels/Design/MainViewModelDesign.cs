using System.Collections.Generic;
using System.Linq;
using Deployer.Library.Generator;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Design
{
    public class MainViewModelDesign : ViewModelBase
    {
        private DeviceViewModelDesign selectedDevice;

        public MainViewModelDesign()
        {
            Devices = DefaultStore.Create()
                .Devices
                .Select(device => new DeviceViewModelDesign())
                .ToList();

            SelectedDevice = Devices.First();
        }

        public List<DeviceViewModelDesign> Devices { get; }

        public DeviceViewModelDesign SelectedDevice
        {
            get => selectedDevice;
            set => this.RaiseAndSetIfChanged(ref selectedDevice, value);
        }

        public StatusMessageViewModel StatusMessage => new SuccessStatusMessageViewModel();

        public bool IsBusy => false;

        public OperationStatusViewModelDesign OperationStatus => new();
    }
}