using System.Reactive;
using ReactiveUI;
using Spectre.Console.Cli;

namespace Deployer.Gui
{
    public class WimFileRequirementViewModelDesign
    {
        public string Description => "This is a Wim File";

        public ReactiveCommand<Unit, Unit> Browse { get; } = ReactiveCommand.Create(() => { });

        public string WimFilePath => "C:\\Users\\blablabla.wim";

        public int WimFileIndex => 1;
    }
}