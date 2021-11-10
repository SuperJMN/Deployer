using System;
using ByteSizeLib;
using ReactiveUI;
using Zafiro.Core;
using Zafiro.Core.ProgressReporting;

namespace Deployer.Gui.ViewModels
{
    public class OperationStatusViewModel : ViewModelBase
    {
        private bool isProgressVisible;
        private bool isProgressIndeterminate;
        private double percentage;

        private readonly ObservableAsPropertyHelper<string> message;

        public OperationStatusViewModel(IDeployer deployer)
        {
            if (deployer == null)
            {
                throw new ArgumentNullException(nameof(deployer));
            }

            message = deployer.Messages.ToProperty(this, x => x.Message);

            deployer.ExecutionContext.Operation.Progress.Subscribe(progress =>
            {
                switch (progress)
                {
                    case Done done:
                        IsProgressVisible = false;
                        ProgressText = "";
                        break;
                    case Percentage p:
                        IsProgressVisible = true;
                        ProgressText = string.Format("{0:P0}", p.Value);
                        IsProgressIndeterminate = false;
                        Percentage = p.Value;
                        break;
                    case AbsoluteProgress<ulong> undefinedProgress:
                        IsProgressVisible = true;
                        IsProgressIndeterminate = true;
                        ProgressText = $"{ByteSize.FromBytes(undefinedProgress.Value):0.0} downloaded";
                        break;
                    case Unknown unknown:
                        IsProgressVisible = true;
                        IsProgressIndeterminate = true;
                        ProgressText = "";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(progress));
                }
            });
        }

        private string progressText;

        public string ProgressText
        {
            get => progressText;
            set => this.RaiseAndSetIfChanged(ref progressText, value);
        }

        public string Message => message.Value;

        public bool IsProgressIndeterminate
        {
            get => isProgressIndeterminate;
            set => this.RaiseAndSetIfChanged(ref isProgressIndeterminate, value);
        }

        public bool IsProgressVisible
        {
            get => isProgressVisible;
            set => this.RaiseAndSetIfChanged(ref isProgressVisible, value);
        }

        public double Percentage
        {
            get => percentage;
            set => this.RaiseAndSetIfChanged(ref percentage, value);
        }
    }
}