using System.IO.Abstractions;
using Autofac;
using Deployer.Console.Core;

namespace Deployer.Console
{
    public static class DeployerConsoleSessionFactory
    {
        public static DeployerSession Create()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileSystem>().AsImplementedInterfaces();
            builder.RegisterType<DeployerConsole>().As<IDeployer>();
            builder.RegisterType<DeployerSession>().AsSelf();
            var container = builder.Build();
            return container.Resolve<DeployerSession>();
        }
    }
}