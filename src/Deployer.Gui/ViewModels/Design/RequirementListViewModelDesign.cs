using System.Collections.Generic;
using Deployer.Gui.ViewModels.Requirements;

namespace Deployer.Gui.ViewModels.Design
{
    public class RequirementListViewModelDesign
    {
        public IList<IRequirementViewModel> Requirements { get; } = new List<IRequirementViewModel>
        {
            new DoubleRequirementViewModel(new DoubleRequirement("Requirement", "This is a double requirement", 1, 12, 20)),
            new WimFileRequirementViewModelDesign(),
            new IntRequirementViewModel(new IntRequirement("Third requirement", "This is a double requirement")),
            new DiskRequirementViewModelDesign(),
        };
    }
}