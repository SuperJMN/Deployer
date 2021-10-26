using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reactive;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Library;
using Iridio.Runtime;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeploymentViewModel
    {
        private readonly Deployment deployment;
        private readonly IFileSystem fileSystem;

        public DeploymentViewModel(Deployment deployment, IDeployer deployer, IFileSystem fileSystem)
        {
            this.deployment = deployment;
            this.fileSystem = fileSystem;

            Deploy = ReactiveCommand.CreateFromTask(() => ExecuteDeployment(deployer));
            Deploy.Subscribe(result =>
            {
                MessageBus.Current.SendMessage(new DeploymentFinished());
                MessageBus.Current.SendMessage(new StatusMessage(result));
            });
        }

        private async Task<string> ExecuteDeployment(IDeployer deployer)
        {
            var deploymentScriptPath = "Deployment-Feed\\" + deployment.ScriptPath;
            var initialState = await CreateInitialState(deploymentScriptPath);

            MessageBus.Current.SendMessage(new DeploymentStart());
            return (await initialState.Bind(async state =>
            {
                var result = await deployer.Run(deploymentScriptPath, state);
                var match = result.Match(s => Result.Success(), d => Result.Failure("Sucked badly"));
                return match;
            })).Match(() => "Success!", err => err);
        }

        private async Task<Result<Dictionary<string, object>>> CreateInitialState(string path)
        {
            var requirementAnalyzer = new RequirementsAnalyzer();
            var requirementFiller = new RequirementFiller();
            var content = await fileSystem.File.ReadAllTextAsync(path);
            return await requirementFiller.Fill(requirementAnalyzer.GetRequirements(content));
        }

        public ReactiveCommand<Unit, string> Deploy { get; set; }

        public string Description => deployment.Description;
        public string Icon => deployment.Icon;
        public string Title => deployment.Title;
    }
}