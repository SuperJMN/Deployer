using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Deployer.Avalonia;
using Deployer.Gui.ViewModels;
using Deployer.Gui.Views;
using Deployer.Library;

namespace Deployer.Gui
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(new XmlDeploymentSerializer(), new DeployerAvalonia())
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
