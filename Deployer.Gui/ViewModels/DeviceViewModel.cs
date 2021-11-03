using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reactive;
using System.Reactive.Linq;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        private readonly Device device;
        private readonly IFileSystem fileSystem;
        private DeploymentViewModel selectedDeployment;
        private readonly ObservableAsPropertyHelper<IList<DeploymentViewModel>> deployments;

        public DeviceViewModel(Device device, IDeployer deployer, IFileSystem fileSystem)
        {
            this.device = device;
            this.fileSystem = fileSystem;
            FetchDeployments = ReactiveCommand.CreateFromObservable( () =>
            {
                return device.Deployments
                    .ToObservable()
                    .SelectMany(deployment => GetRequirements(deployment)
                        .Select(requirements => new DeploymentViewModel(deployment, deployer, requirements)))
                    .ToList();
            });

            deployments = FetchDeployments.ToProperty(this, x => x.Deployments);
            FetchDeployments.Execute().Subscribe();
        }

        public IList<DeploymentViewModel> Deployments => deployments.Value;

        public ReactiveCommand<Unit, IList<DeploymentViewModel>> FetchDeployments { get; set; }

        private IObservable<IEnumerable<Requirement>> GetRequirements(Deployment deployment)
        {
            var requirementAnalyzer = new RequirementsAnalyzer();
            var deploymentScriptPath = "Deployment-Feed\\" + deployment.ScriptPath;

            return Observable
                .FromAsync(async () =>
                    requirementAnalyzer.GetRequirements(await fileSystem.File.ReadAllTextAsync(deploymentScriptPath)));
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

    }
}