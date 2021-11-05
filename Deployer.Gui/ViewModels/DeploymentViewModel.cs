using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Gui.ViewModels.Messages;
using Deployer.Library;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeploymentViewModel : ViewModelBase
    {
        private readonly Deployment deployment;

        public DeploymentViewModel(Deployment deployment, IDeployer deployer,
            IEnumerable<Requirement> requirements)
        {
            this.deployment = deployment;

            Deploy = ReactiveCommand.CreateFromTask(() => ExecuteDeployment(deployer));
            Deploy.Subscribe(result =>
            {
                MessageBus.Current.SendMessage(new DeploymentFinished());
                MessageBus.Current.SendMessage(new StatusMessage(result));
            });

            Requirements = new RequirementListViewModel(requirements);
        }

        public RequirementListViewModel Requirements { get; }

        public ReactiveCommand<Unit, string> Deploy { get; }

        public string Description => deployment.Description;
        public string Icon => deployment.Icon;
        public string Title => deployment.Title;

        private async Task<string> ExecuteDeployment(IDeployer deployer)
        {
            var deploymentScriptPath = "Deployment-Feed\\" + deployment.ScriptPath;
            var initialState = CreateInitialState();

            MessageBus.Current.SendMessage(new DeploymentStart());
            return (await initialState.Bind(async state =>
            {
                var result = await deployer.Run(deploymentScriptPath, state);
                var match = result.Match(s => Result.Success(), d => Result.Failure(d.ToString()));
                return match;
            })).Match(() => "Success!", err => err);
        }

        private Result<Dictionary<string, object>> CreateInitialState()
        {
            var reqs = Requirements.Requirements.SelectMany(r => r.FilledRequirements);
            var dict = reqs.ToDictionary(r => r.Item1, r => r.Item2);

            return dict;
        }
    }
}