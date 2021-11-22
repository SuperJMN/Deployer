using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Gui.ViewModels.Messages;
using Deployer.Library;
using Deployer.Wim;
using ReactiveUI;
using Zafiro.Storage;

namespace Deployer.Gui.ViewModels
{
    public class DeploymentViewModel : ViewModelBase
    {
        private readonly Deployment deployment;
        private readonly IFileSystem fileSystem;

        public DeploymentViewModel(Deployment deployment, IDeployer deployer,
            IEnumerable<Requirement> requirements, IFileSystem fileSystem,
            IWindowsImageMetadataReader windowsImageMetadataReader, IStorage storage)
        {
            this.deployment = deployment;
            this.fileSystem = fileSystem;

            Requirements = new RequirementListViewModel(requirements, windowsImageMetadataReader, fileSystem, storage);

            Deploy = ReactiveCommand.CreateFromTask(() => ExecuteDeployment(deployer), Requirements.IsValid);
            Deploy.Subscribe(result =>
            {
                MessageBus.Current.SendMessage(new DeploymentFinished());
                MessageBus.Current.SendMessage(result.Match(()=> (StatusMessageViewModel)new SuccessStatusMessageViewModel(), err => new ErrorStatusMessageViewModel(err)));
            });
        }

        public RequirementListViewModel Requirements { get; }

        public ReactiveCommand<Unit, Result> Deploy { get; }

        public string Description => deployment.Description;
        public string Icon => deployment.Icon;
        public string Title => deployment.Title;

        private async Task<Result> ExecuteDeployment(IDeployer deployer)
        {
            var deploymentScriptPath = fileSystem.Path.Combine(Constants.GetDeploymentFeedPath(fileSystem), deployment.ScriptPath);
            var initialState = CreateInitialState();

            MessageBus.Current.SendMessage(new DeploymentStart());
            return (await initialState.Bind(async state =>
            {
                var result = await deployer.Run(deploymentScriptPath, state);
                var match = result.Match(s => Result.Success(), d => Result.Failure(d.ToString()));
                return match;
            })).Match(Result.Success, Result.Failure);
        }

        private Result<Dictionary<string, object>> CreateInitialState()
        {
            var reqs = Requirements.Requirements.SelectMany(r => r.FilledRequirements);
            var dict = reqs.ToDictionary(r => r.Item1, r => r.Item2);

            return dict;
        }
    }
}