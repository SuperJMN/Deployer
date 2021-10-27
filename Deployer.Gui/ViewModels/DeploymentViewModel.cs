using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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

            Requirements = new RequirementsViewModel(deployment, fileSystem);
        }

        public RequirementsViewModel Requirements { get; }

        private async Task<string> ExecuteDeployment(IDeployer deployer)
        {
            var deploymentScriptPath = "Deployment-Feed\\" + deployment.ScriptPath;
            var initialState = await CreateInitialState(deploymentScriptPath);

            MessageBus.Current.SendMessage(new DeploymentStart());
            return (await initialState.Bind(async state =>
            {
                var result = await deployer.Run(deploymentScriptPath, state);
                var match = result.Match(s => Result.Success(), d => Result.Failure(d.ToString()));
                return match;
            })).Match(() => "Success!", err => err);
        }

        private async Task<Result<Dictionary<string, object>>> CreateInitialState(string path)
        {
            var requirementAnalyzer = new RequirementsAnalyzer();
            var content = await fileSystem.File.ReadAllTextAsync(path);
            var requirements = requirementAnalyzer.GetRequirements(content).ToList();

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

    public class RequirementsViewModel : ViewModelBase
    {
        private readonly Deployment deployment;
        private readonly IFileSystem fileSystem;
        private readonly ObservableAsPropertyHelper<IEnumerable<ViewModelBase>> requirements;
        private readonly RequirementsAnalyzer requirementAnalyzer;

        public RequirementsViewModel(Deployment deployment, IFileSystem fileSystem)
        {
            this.deployment = deployment ?? throw new ArgumentNullException(nameof(deployment));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

            requirementAnalyzer = new RequirementsAnalyzer();
            Load = ReactiveCommand.CreateFromObservable(GetRequirementsObservable);
            requirements = Load.ToProperty(this, x => x.Requirements);
            Load.Execute().Subscribe();
        }

        private IObservable<IEnumerable<ViewModelBase>> GetRequirementsObservable()
        {
            return Observable.FromAsync(async () =>
                {
                    var deploymentScriptPath = "Deployment-Feed\\" + deployment.ScriptPath;
                    var content = await fileSystem.File.ReadAllTextAsync(deploymentScriptPath);
                    var reqs = requirementAnalyzer.GetRequirements(content);
                    return reqs;
                })
                .Select(list => list.Select(GetViewModel));
        }

        public IEnumerable<ViewModelBase> Requirements => requirements.Value;

        private ViewModelBase GetViewModel(Requirement requirement)
        {
            switch (requirement)
            {
                case DoubleRequirement doubleRequirement:
                    return new DoubleRequirementViewModel(doubleRequirement);
                case IntRequirement intRequirement:
                    return new IntRequirementViewModel(intRequirement);
                case WimFileRequirement wimFileRequirement:
                    return new WimFileRequirementViewModel(wimFileRequirement);
                default:
                    throw new ArgumentOutOfRangeException(nameof(requirement));
            }
        }

        private static async Task<IEnumerable<Requirement>> GetRequirements(Deployment deployment, IFileSystem fileSystem,
            RequirementsAnalyzer? requirementAnalyzer)
        {
            var content = await fileSystem.File.ReadAllTextAsync(deployment.ScriptPath);
            var requirements = requirementAnalyzer.GetRequirements(content);
            return requirements;
        }

        public ReactiveCommand<Unit, IEnumerable<ViewModelBase>> Load { get; set; }
    }
}