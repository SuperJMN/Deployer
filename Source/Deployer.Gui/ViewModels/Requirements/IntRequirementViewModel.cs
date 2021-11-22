using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Requirements
{
    public class IntRequirementViewModel : RequirementViewModelBase
    {
        private int value;
        public IntRequirement Requirement { get; }

        public IntRequirementViewModel(IntRequirement requirement) : base(requirement)
        {
            Requirement = requirement;
        }

        public int Value
        {
            get => this.value;
            set => this.RaiseAndSetIfChanged(ref this.value, value);
        }

        public override IEnumerable<(string, object)> FilledRequirements => new[] { (Requirement.Key, (object)Value) };
        public override IObservable<bool> IsValid => Observable.Return(true);
    }
}