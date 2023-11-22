using System.IO.Abstractions;
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Deployer.Avalonia;
using Deployer.Gui.Services;
using Deployer.Gui.ViewModels;
using Deployer.Gui.Views;
using Deployer.Library;
using Deployer.Wim;
using Zafiro.Storage.Windows;

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
                desktopMainWindow.DataContext = CreateDataContext();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static MainWindowViewModel CreateDataContext()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<DeployerAvalonia>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<FeedInstaller>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<DeviceRepository>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<XmlDeploymentSerializer>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<WindowsImageMetadataReader>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<FileSystem>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<Storage>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterAssemblyTypes(typeof(ViewModelBase).Assembly).Where(t => t.IsAssignableTo(typeof(ViewModelBase))).AsSelf();

            var container = containerBuilder.Build();
            var main = container.Resolve<MainWindowViewModel>();

            return main;
        }
    }
}
