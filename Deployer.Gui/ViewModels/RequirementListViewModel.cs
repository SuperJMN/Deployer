using System;
using System.Collections.Generic;
using System.Linq;
using Deployer.Gui.ViewModels.Requirements;

namespace Deployer.Gui.ViewModels
{
    public class RequirementListViewModel : ViewModelBase
    {
        public RequirementListViewModel(IEnumerable<Requirement> requirements)
        {
            Requirements = requirements.Select(GetViewModel).ToList();
        }

        public IList<RequirementViewModelBase> Requirements { get; }

        private RequirementViewModelBase GetViewModel(Requirement requirement)
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
}