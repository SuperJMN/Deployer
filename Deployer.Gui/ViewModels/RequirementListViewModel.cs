using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Deployer.Gui.ViewModels.Requirements;

namespace Deployer.Gui.ViewModels
{
    public class RequirementListViewModel : ViewModelBase
    {
        public RequirementListViewModel(IEnumerable<Requirement> requirements)
        {
            Requirements = requirements.Select(GetViewModel).ToList();
            IsValid = Requirements
                .Select(x => x.IsValid)
                .CombineLatest(list => list.All(b => b));
        }

        public IList<RequirementViewModelBase> Requirements { get; }
        public IObservable<bool> IsValid { get; }

        private static RequirementViewModelBase GetViewModel(Requirement requirement)
        {
            switch (requirement)
            {
                case DoubleRequirement doubleRequirement:
                    return new DoubleRequirementViewModel(doubleRequirement);
                case DiskRequirement diskRequirement:
                    return new DiskRequirementViewModel(diskRequirement);
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