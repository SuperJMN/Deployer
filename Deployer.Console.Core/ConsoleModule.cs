using Autofac;
using Deployer.Functions.Services;

namespace Deployer.Console.Core
{
    public class ConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MarkdownService>().As<IMarkdownService>();
        }
    }
}