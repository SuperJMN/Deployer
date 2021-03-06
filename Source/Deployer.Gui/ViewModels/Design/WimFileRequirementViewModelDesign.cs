using System.Collections.Generic;
using System.Reactive;
using CSharpFunctionalExtensions;
using Deployer.Wim;
using ReactiveUI;
using Zafiro.System.Windows;

namespace Deployer.Gui.ViewModels.Design
{
    public class WimFileRequirementViewModelDesign : IRequirementViewModel
    {
        public string Description => "This is a Wim File";
        public ReactiveCommand<Unit, Unit> Browse { get; } = ReactiveCommand.Create(() => { });
        public string WimFilePath => "C:\\Users\\blablabla.wim";
        public int WimFileIndex => 1;

        public IEnumerable<DiskImageMetadata> Images => new[]
        {
            new DiskImageMetadata
            {
                Index = 1,
                Build = "123.456",
                Architecture = ProcessorArchitecture.Arm64,
                DisplayName = "Windows 10 Pro"
            },
        };

        public Maybe<DiskImageMetadata> SelectedImage => Images.TryFirst();
    }
}