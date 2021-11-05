using System.Reactive;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Design
{
    public class WimFileRequirementViewModelDesign
    {
        public string Description => "This is a Wim File";

        public ReactiveCommand<Unit, Unit> Browse { get; } = ReactiveCommand.Create(() => { });

        public string WimFilePath => "C:\\Users\\blablabla.wim";

        public int WimFileIndex => 1;
    }
}