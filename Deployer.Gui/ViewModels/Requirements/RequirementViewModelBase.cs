using System;
using System.Collections.Generic;

namespace Deployer.Gui.ViewModels.Requirements
{
    public abstract class RequirementViewModelBase : ViewModelBase
    {
        private readonly Requirement requirement;

        public RequirementViewModelBase(Requirement requirement)
        {
            this.requirement = requirement;
        }

        public string Name => requirement.Key;
        public string Description => requirement.Description;
        public abstract IEnumerable<(string, object)> FilledRequirements { get; }
        public abstract IObservable<bool> IsValid { get; }
    }
}