using System.Reactive;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeploymentViewModel
    {
        private readonly Deployment deployment;

        public DeploymentViewModel(Deployment deployment)
        {
            this.deployment = deployment;
            Deploy = ReactiveCommand.CreateFromTask(async () => { });
        }

        public ReactiveCommand<Unit, Unit> Deploy { get; set; }

        public string Description => deployment.Description;
        public string Icon => deployment.Icon;
        public string Title => deployment.Title;
    }
}