using Autofac;
using Deployer.Gui.ViewModels;
using Deployer.Library;

namespace Deployer.Gui
{
    public class Composition
    {
        public static IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<XmlDeploymentSerializer>().As<IDeployementSerializer>();
            containerBuilder.RegisterAssemblyTypes(typeof(MainWindowViewModel).Assembly)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .AsSelf();

            return containerBuilder.Build();
        }
    }
}