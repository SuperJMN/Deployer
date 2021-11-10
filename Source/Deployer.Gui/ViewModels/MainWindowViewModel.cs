using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using Deployer.Gui.ViewModels.Messages;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string SupportLink = "https://github.com/sponsors/SuperJMN";
        private readonly ObservableAsPropertyHelper<List<DeviceViewModel>> devices;
        private bool isBusy;
        private DeviceViewModel selectedDevice = null!;
        private StatusMessageViewModel statusMessage = null!;

        public MainWindowViewModel(Func<Device, DeviceViewModel> deviceViewModelFactory, IFeedInstaller feedInstaller,
            IDeviceRepository repository, OperationStatusViewModel operationStatus)
        {
            OperationStatus = operationStatus;

            Install = ReactiveCommand.CreateFromTask(feedInstaller.Install);
            Fetch = CreateFetch(deviceViewModelFactory, repository);
            devices = Fetch.ToProperty(this, x => x.Devices);

            MessageBus.Current.Listen<StatusMessageViewModel>().Subscribe(m => StatusMessage = m);
            MessageBus.Current.Listen<DeploymentStart>().Subscribe(m =>
            {
                StatusMessage = null;
                IsBusy = true;
            });
            MessageBus.Current.Listen<DeploymentFinished>().Subscribe(m => IsBusy = false);

            SupportMyWork = ReactiveCommand.Create(() =>
            {
                var ps = new ProcessStartInfo(SupportLink)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            });

            Fetch.Execute().Subscribe();
        }

        public OperationStatusViewModel OperationStatus { get; }

        public ReactiveCommand<Unit, Result> Install { get; }

        public bool IsBusy
        {
            get => isBusy;
            private set => this.RaiseAndSetIfChanged(ref isBusy, value);
        }


        public List<DeviceViewModel> Devices => devices.Value;

        public ReactiveCommand<Unit, List<DeviceViewModel>> Fetch { get; }

        public ReactiveCommand<Unit, Unit> SupportMyWork { get; }

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

        private ReactiveCommand<Unit, List<DeviceViewModel>> CreateFetch(
            Func<Device, DeviceViewModel> deviceViewModelFactory, IDeviceRepository repository)
        {
            return ReactiveCommand.CreateFromObservable(() =>
                Install.Execute()
                    .Select(result => result.Match(repository.Get, _ => Enumerable.Empty<Device>())
                        .Select(deviceViewModelFactory)
                        .ToList()));
        }
    }
}