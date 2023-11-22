using System;

namespace Deployer.Gui.ViewModels
{
    public class ErrorStatusMessageViewModel : StatusMessageViewModel
    {
        public string Message { get; }

        public ErrorStatusMessageViewModel(string message)
        {
            Message = message;
        }
    }
}