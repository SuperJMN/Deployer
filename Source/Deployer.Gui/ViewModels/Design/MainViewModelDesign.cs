using System.Collections.Generic;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Design
{
    public class MainViewModelDesign : ViewModelBase
    {
        private DeviceViewModelDesign selectedDevice;

        public MainViewModelDesign()
        {
            var deviceViewModelDesign = new DeviceViewModelDesign();
            Devices = new List<DeviceViewModelDesign>()
            {
                deviceViewModelDesign,
            };

            SelectedDevice = deviceViewModelDesign;
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