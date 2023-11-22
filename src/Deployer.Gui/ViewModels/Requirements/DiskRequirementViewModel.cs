using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Zafiro.Storage;

namespace Deployer.Gui.ViewModels.Requirements
{
    public class DiskRequirementViewModel : IntRequirementViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> isBusy;
        private DiskViewModel selectedDisk;
        private readonly ObservableAsPropertyHelper<IEnumerable<DiskViewModel>> disks;
        private bool isUnlocked;

        public DiskRequirementViewModel(IntRequirement diskRequirement, IStorage storage) : base(diskRequirement)
        {
            RefreshDisks = ReactiveCommand.CreateFromTask(storage.GetDisks);
            disks = RefreshDisks
                .Select(x => x.Select(disk => new DiskViewModel(disk))
                    .OrderBy(d => d.Number))
                .ToProperty(this, x => x.Disks);


            isBusy = RefreshDisks.IsExecuting.ToProperty(this, x => x.IsBusy);
            IsValid = this.WhenAnyValue(x => x.SelectedDisk, x => x.IsUnlocked).Select(model => model.Item1 is not null && model.Item2);
            RefreshDisks.Execute().Subscribe();
            this.WhenAnyValue(x => x.SelectedDisk)
                .Where(d => d is not null)
                .Subscribe(disk => Value = (int) disk.Number);
        }

        public bool IsBusy => isBusy.Value;

        public IEnumerable<DiskViewModel> Disks => disks.Value;

        public ReactiveCommand<Unit, IList<IDisk>> RefreshDisks { get; }

        public DiskViewModel SelectedDisk
        {
            get => selectedDisk;
            set => this.RaiseAndSetIfChanged(ref selectedDisk, value);
        }

        public bool IsUnlocked
        {
            get => isUnlocked;
            set => this.RaiseAndSetIfChanged(ref isUnlocked, value);
        }

        public override IObservable<bool> IsValid { get; }
    }
}