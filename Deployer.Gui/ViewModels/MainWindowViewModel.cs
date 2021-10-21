using System;
using System.IO;
using System.Reactive;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Func<XmlDeploymentReader> func;

        public MainWindowViewModel(IDeployementReader deploymentReader)
        {
            Fetch = ReactiveCommand.Create(() => deploymentReader.Read(File.ReadAllText("Deployments.xml")));
            Fetch.Subscribe(store => { });
        }

        public ReactiveCommand<Unit, DeployerStore> Fetch { get; }

        public string Greeting => "Welcome to Avalonia!";

        public Deployment Deployment { get; set; }
    }

    public class Deployment
    {
    }
}
