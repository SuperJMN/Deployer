namespace Deployer.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public Deployment Deployment { get; set; }
    }

    public class Deployment
    {
    }
}
