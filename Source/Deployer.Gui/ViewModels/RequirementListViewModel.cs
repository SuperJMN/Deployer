using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive.Linq;
using Deployer.Gui.ViewModels.Requirements;
using Deployer.Wim;
using Zafiro.Storage;

namespace Deployer.Gui.ViewModels
{
    public class RequirementListViewModel : ViewModelBase
    {
        private readonly IWindowsImageMetadataReader windowsImageMetadataReader;
        private readonly IFileSystem fileSystem;
        private readonly IStorage storage;

        public RequirementListViewModel(IEnumerable<Requirement> requirements, IWindowsImageMetadataReader windowsImageMetadataReader, IFileSystem fileSystem, IStorage storage)
        {
            this.windowsImageMetadataReader = windowsImageMetadataReader;
            this.fileSystem = fileSystem;
            this.storage = storage;
            Requirements = requirements.Select(GetViewModel).ToList();
            IsValid = Requirements
                .Select(x => x.IsValid)
                .CombineLatest(list => list.All(b => b));
        }

        public IList<RequirementViewModelBase> Requirements { get; }
        public IObservable<bool> IsValid { get; }

        private RequirementViewModelBase GetViewModel(Requirement requirement)
        {
            switch (requirement)
            {
                case DoubleRequirement doubleRequirement:
                    return new DoubleRequirementViewModel(doubleRequirement);
                case DiskRequirement diskRequirement:
                    return new DiskRequirementViewModel(diskRequirement, storage);
                case IntRequirement intRequirement:
                    return new IntRequirementViewModel(intRequirement);
                case WimFileRequirement wimFileRequirement:
                    return new WimFileRequirementViewModel(wimFileRequirement, windowsImageMetadataReader, fileSystem);
                default:
                    throw new ArgumentOutOfRangeException(nameof(requirement));
            }
        }
    }
}