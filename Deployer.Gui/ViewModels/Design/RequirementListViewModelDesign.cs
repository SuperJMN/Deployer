using System.Collections.Generic;
using Deployer.Gui.ViewModels.Requirements;

namespace Deployer.Gui.ViewModels.Design
{
    public class RequirementListViewModelDesign
    {
        public IList<RequirementViewModelBase> Requirements { get; } = new List<RequirementViewModelBase>
        {
            new DoubleRequirementViewModel(new DoubleRequirement("Requirement", "This is a double requirement", 1, 12, 20)),
            new WimFileRequirementViewModel(new WimFileRequirement("Another", "This is a double requirement")),
            new IntRequirementViewModel(new IntRequirement("Third requirement", "This is a double requirement"))
        };
    }
}