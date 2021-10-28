using System.Collections.Generic;

namespace Deployer.Gui.ViewModels.Requirements
{
    public abstract class RequirementViewModelBase : ViewModelBase
    {
        public abstract IEnumerable<(string, object)> FilledRequirements { get; }
    }
}