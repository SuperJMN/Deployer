using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DeviceViewModel selectedDevice;
        private readonly ObservableAsPropertyHelper<List<DeviceViewModel>> devices;

        public MainWindowViewModel(IDeployementSerializer deploymentSerializer, IDeployer deployer)
        {
            Fetch = ReactiveCommand.Create(() =>
            {
                return deploymentSerializer
                    .Deserialize(File.ReadAllText("Store.xml"))
                    .Devices
                    .Select(device => new DeviceViewModel(device, deployer))
                    .ToList();
            });

            devices = Fetch.ToProperty(this, x => x.Devices);

            Fetch.Execute().Subscribe();
        }

        public List<DeviceViewModel> Devices => devices.Value;

        public ReactiveCommand<Unit, List<DeviceViewModel>> Fetch { get; }

        public DeviceViewModel SelectedDevice
        {
            get => selectedDevice;
            set => this.RaiseAndSetIfChanged(ref selectedDevice, value);
        }
    }
}
