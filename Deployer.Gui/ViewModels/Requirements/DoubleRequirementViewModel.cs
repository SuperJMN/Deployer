using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Requirements
{
    internal class DoubleRequirementViewModel : RequirementViewModelBase
    {
        private double value;
        public DoubleRequirement Requirement { get; }

        public DoubleRequirementViewModel(DoubleRequirement requirement) : base(requirement)
        {
            Requirement = requirement;
            Value = requirement.DefaultValue;
            Minimum = requirement.Min;
            Maximum = requirement.Max;
            IsValid = this.WhenAnyValue(model => model.Value).Select(v => v >= requirement.Min && v <= requirement.Max);
        }

        public double Minimum { get; }

        public double Maximum { get; }

        public double Value
        {
            get => value;
            set => this.RaiseAndSetIfChanged(ref this.value, value);
        }

        public override IEnumerable<(string, object)> FilledRequirements => new[] {(Requirement.Key, (object) Value)};
        public override IObservable<bool> IsValid { get; }
    }
}