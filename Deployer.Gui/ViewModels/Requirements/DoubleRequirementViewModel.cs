using System.Collections.Generic;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Requirements
{
    internal class DoubleRequirementViewModel : RequirementViewModelBase
    {
        private double value;
        public DoubleRequirement Requirement { get; }

        public DoubleRequirementViewModel(DoubleRequirement requirement)
        {
            Requirement = requirement;
        }

        public double Value
        {
            get => value;
            set => this.RaiseAndSetIfChanged(ref this.value, value);
        }

        public override IEnumerable<(string, object)> FilledRequirements => new[] {(Requirement.Key, (object) Value)};
    }
}