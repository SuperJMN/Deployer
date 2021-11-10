using Autofac;
using Deployer.Functions.Services;

namespace Deployer.Avalonia
{
    public class AvaloniaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MarkdownService>().As<IMarkdownService>();
        }
    }
}