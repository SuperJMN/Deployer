using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using Zafiro.Storage;

namespace Deployer.Gui.ViewModels.Design
{
    public interface IDiskRequirementViewModel : IRequirementViewModel
    {
        bool IsBusy { get; }
        IEnumerable<IDiskViewModel> Disks { get; }
        ReactiveCommand<Unit, IList<IDisk>> RefreshDisks { get; }
        IDiskViewModel SelectedDisk { get; }
        bool IsUnlocked { get; }
    }
}