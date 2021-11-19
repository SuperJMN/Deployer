using System;
using System.Collections.Generic;
using Deployer.Gui.ViewModels.Design;

namespace Deployer.Gui.ViewModels.Requirements
{
    public abstract class RequirementViewModelBase : ViewModelBase, IRequirementViewModel
    {
        private protected RequirementViewModelBase(Requirement requirement)
        {
            Requirement = requirement;
        }

        private Requirement Requirement { get; }

        public string Name => Requirement.Key;
        public string Description => Requirement.Description;

        public abstract IEnumerable<(string, object)> FilledRequirements { get; }
        public abstract IObservable<bool> IsValid { get; }
    }
}