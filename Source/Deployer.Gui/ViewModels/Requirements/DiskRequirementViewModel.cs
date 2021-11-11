using System;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Requirements
{
    class DiskRequirementViewModel : IntRequirementViewModel
    {
        private bool isUnlocked;

        public DiskRequirementViewModel(IntRequirement requirement) : base(requirement)
        {
            IsValid = this.WhenAnyValue(x => x.IsUnlocked);
        }

        public bool IsUnlocked
        {
            get => isUnlocked;
            set => this.RaiseAndSetIfChanged(ref isUnlocked, value);
        }

        public override IObservable<bool> IsValid { get; }
    }
}