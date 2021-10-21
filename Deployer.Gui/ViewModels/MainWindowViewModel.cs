using System.IO;
using System.Reactive;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<DeployerStore> deployerStore;
        private Deployment selectedDeployment;
        private Device selectedDevice;

        public MainWindowViewModel(IDeployementSerializer deploymentSerializer)
        {
            Fetch = ReactiveCommand.Create(() => deploymentSerializer.Deserialize(File.ReadAllText("Store.xml")));
            deployerStore = Fetch.ToProperty(this, m => m.DeployerStore);
        }

        public DeployerStore DeployerStore => deployerStore.Value;

        public ReactiveCommand<Unit, DeployerStore> Fetch { get; }

        public string Greeting => "Welcome to Avalonia!";

        public Device SelectedDevice
        {
            get => selectedDevice;
            set => this.RaiseAndSetIfChanged(ref selectedDevice, value);
        }

        public Deployment SelectedDeployment
        {
            get => selectedDeployment;
            set => this.RaiseAndSetIfChanged(ref selectedDeployment, value);
        }
    }
}
