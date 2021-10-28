using System.Collections.Generic;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Requirements
{
    internal class IntRequirementViewModel : RequirementViewModelBase
    {
        private int value;
        public IntRequirement Requirement { get; }

        public IntRequirementViewModel(IntRequirement requirement)
        {
            Requirement = requirement;
        }

        public int Value
        {
            get => this.value;
            set => this.RaiseAndSetIfChanged(ref this.value, value);
        }

        public override IEnumerable<(string, object)> FilledRequirements => new[] { (Requirement.Key, (object)Value) };
    }
}