using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using CSharpFunctionalExtensions;
using Deployer.Gui.Views;
using MoreLinq;
using ReactiveUI;

namespace Deployer.Gui.ViewModels
{
    public class RequirementFiller : ViewModelBase
    {
        private readonly ContentControl host;
        private readonly Func<Control> toLoad;

        public RequirementFiller(ContentControl host, Func<Control> toLoad)
        {
            this.host = host;
            this.toLoad = toLoad;
        }

        public Task<Result<Dictionary<string, object>>> Fill(IEnumerable<Requirement> getRequirements)
        {
            var content = toLoad();
            host.Content = content;
            content.DataContext = getRequirements.Select(GetViewModel).ToList();

            var tcs = new TaskCompletionSource<Result<Dictionary<string, object>>>();

            var dictionary = new Dictionary<string, object>
            {
                ["Disk"] = 4,
                ["DeploymentSize"] = 16D,
                ["WimFileIndex"] = 1,
                ["WimFilePath"] = "J:\\sources\\install.wim"
            };

            return tcs.Task;
        }

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
    }

    internal class WimFileRequirementViewModel : ViewModelBase
    {
        public WimFileRequirement WimFileRequirement { get; }

        public WimFileRequirementViewModel(WimFileRequirement wimFileRequirement)
        {
            WimFileRequirement = wimFileRequirement;
        }
    }

    internal class IntRequirementViewModel : ViewModelBase
    {
        private int value;
        public IntRequirement IntRequirement { get; }

        public IntRequirementViewModel(IntRequirement intRequirement)
        {
            IntRequirement = intRequirement;
        }

        public int Value
        {
            get => value;
            set => this.RaiseAndSetIfChanged(ref value, value);
        }
    }

    internal class DoubleRequirementViewModel : ViewModelBase
    {
        public DoubleRequirement DoubleRequirement { get; }

        public DoubleRequirementViewModel(DoubleRequirement doubleRequirement)
        {
            DoubleRequirement = doubleRequirement;
        }
    }
}