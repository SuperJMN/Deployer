using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Deployer.Gui.ViewModels.Requirements;
using ReactiveUI;
using Zafiro.Storage;

namespace Deployer.Gui.ViewModels.Design
{
    public class DiskRequirementViewModelDesign : IDiskRequirementViewModel
    {
        private readonly IDiskViewModel[] diskViewModels =
        {
            new DiskViewModelDesign(),
            new DiskViewModelDesign(),
            new DiskViewModelDesign(),
        };
        public bool IsBusy => false;

        public IEnumerable<IDiskViewModel> Disks => diskViewModels;

        public ReactiveCommand<Unit, IList<IDisk>> RefreshDisks { get; }

        public IDiskViewModel SelectedDisk => diskViewModels.First();

        public bool IsUnlocked => false;
        public string Description => "This is a disk";
    }
}