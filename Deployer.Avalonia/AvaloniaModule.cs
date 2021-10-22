using System;
using System.Threading.Tasks;
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

    public class MarkdownService : IMarkdownService
    {
        public Task FromFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}