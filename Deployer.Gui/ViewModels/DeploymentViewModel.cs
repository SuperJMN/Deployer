using System;
using System.Collections.Generic;
using System.Reactive;
using CSharpFunctionalExtensions;
using Deployer.Library;
using Iridio.Runtime;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class DeploymentViewModel
    {
        private readonly Deployment deployment;

        public DeploymentViewModel(Deployment deployment, IDeployer deployer)
        {
            this.deployment = deployment;
            Deploy = ReactiveCommand.CreateFromTask(() =>
                deployer.Run("Deployment-Feed\\" + deployment.ScriptPath, new Dictionary<string, object>
                {
                    ["Disk"] = 4,
                    ["DeploymentSize"] = 16D,
                    ["WimFileIndex"] = 1,
                    ["WimFilePath"] = "fake"
                }));
            Deploy.Subscribe(r => { });
        }

        public ReactiveCommand<Unit, Result<ExecutionSummary, IridioError>> Deploy { get; set; }

        public string Description => deployment.Description;
        public string Icon => deployment.Icon;
        public string Title => deployment.Title;
    }
}