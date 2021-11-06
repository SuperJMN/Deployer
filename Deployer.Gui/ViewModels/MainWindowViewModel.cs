using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Deployer.Gui.ViewModels.Messages;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public OperationStatusViewModel OperationStatus { get; }
        private DeviceViewModel selectedDevice = null!;
        private readonly ObservableAsPropertyHelper<List<DeviceViewModel>> devices;
        private StatusMessageViewModel statusMessage = null!;
        private bool isBusy;

        public MainWindowViewModel(Func<Device, DeviceViewModel> deviceViewModelFactory, IFeedInstaller feedInstaller,
            IDeviceRepository repository, OperationStatusViewModel operationStatus)
        {
            OperationStatus = operationStatus;

            Install = ReactiveCommand.CreateFromTask(feedInstaller.Install);
            Install.Execute().Subscribe();

            Fetch = ReactiveCommand.CreateFromObservable(() =>
                Install.Execute()
                    .Select(_ => repository.Get()
                        .Select(deviceViewModelFactory)
                        .ToList()));

            devices = Fetch.ToProperty(this, x => x.Devices);

            Fetch.Execute().Subscribe();
            MessageBus.Current.Listen<StatusMessageViewModel>().Subscribe(m => StatusMessage = m);
            MessageBus.Current.Listen<DeploymentStart>().Subscribe(m =>
            {
                StatusMessage = null;
                IsBusy = true;
            });


            MessageBus.Current.Listen<DeploymentFinished>().Subscribe(m => IsBusy = false);
        }

        public ReactiveCommand<Unit, Unit> Install { get; }

        public bool IsBusy
        {
            get => isBusy;
            private set => this.RaiseAndSetIfChanged(ref isBusy, value);
        }

        public List<DeviceViewModel> Devices => devices.Value;

        public ReactiveCommand<Unit, List<DeviceViewModel>> Fetch { get; }

        public DeviceViewModel SelectedDevice
        {
            get => selectedDevice;
            set => this.RaiseAndSetIfChanged(ref selectedDevice, value);
        }

        public StatusMessageViewModel StatusMessage
        {
            get => statusMessage;
            private set => this.RaiseAndSetIfChanged(ref statusMessage, value);
        }
    }
}
