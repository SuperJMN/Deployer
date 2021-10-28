using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeploymentViewModel : ViewModelBase
    {
        private readonly Deployment deployment;
        private readonly IFileSystem fileSystem;

        public DeploymentViewModel(Deployment deployment, IDeployer deployer, IFileSystem fileSystem,
            IEnumerable<Requirement> requirements)
        {
            this.deployment = deployment;
            this.fileSystem = fileSystem;

            Deploy = ReactiveCommand.CreateFromTask(() => ExecuteDeployment(deployer));
            Deploy.Subscribe(result =>
            {
                MessageBus.Current.SendMessage(new DeploymentFinished());
                MessageBus.Current.SendMessage(new StatusMessage(result));
            });

            Requirements = new RequirementListViewModel(requirements);
        }

        public RequirementListViewModel Requirements { get; }

        private async Task<string> ExecuteDeployment(IDeployer deployer)
        {
            var deploymentScriptPath = "Deployment-Feed\\" + deployment.ScriptPath;
            var initialState = await CreateInitialState();

            MessageBus.Current.SendMessage(new DeploymentStart());
            return (await initialState.Bind(async state =>
            {
                var result = await deployer.Run(deploymentScriptPath, state);
                var match = result.Match(s => Result.Success(), d => Result.Failure(d.ToString()));
                return match;
            })).Match(() => "Success!", err => err);
        }

        private async Task<Result<Dictionary<string, object>>> CreateInitialState()
        {
            var reqs = Requirements.Requirements.SelectMany(r => r.FilledRequirements);
            var dict = reqs.ToDictionary(r => r.Item1, r => r.Item2);
            
            var dictionary = new Dictionary<string, object>
            {
                ["Disk"] = 4,
                ["DeploymentSize"] = 16D,
                ["WimFileIndex"] = 1,
                ["WimFilePath"] = "J:\\sources\\install.wim"
            };

            return dictionary;
        }

        public ReactiveCommand<Unit, string> Deploy { get; set; }

        public string Description => deployment.Description;
        public string Icon => deployment.Icon;
        public string Title => deployment.Title;
    }
}