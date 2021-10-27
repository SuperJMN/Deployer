using System.IO.Abstractions;
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
                var desktopMainWindow = new MainWindow();
                desktop.MainWindow = desktopMainWindow;

                var deployerAvalonia = new DeployerAvalonia();
                var mainWindowViewModel = new MainWindowViewModel(new XmlDeploymentSerializer(), new OperationStatusViewModel(deployerAvalonia),
                    deployerAvalonia, new FileSystem());

                desktopMainWindow.DataContext = mainWindowViewModel;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
