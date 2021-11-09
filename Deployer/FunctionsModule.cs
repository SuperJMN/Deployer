using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO.Abstractions;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deployer.Compression;
using Deployer.Functions;
using Deployer.Functions.Core;
using Iridio.Common;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq.Extensions;
using Octokit;
using Refit;
using Zafiro.Core.FileSystem;
using Zafiro.Network;
using Zafiro.Storage;
using Zafiro.Storage.Windows;
using Zafiro.System.Windows;
using Zafiro.System.Windows.Dism;
using Zafiro.Tools.AzureDevOps;

namespace Deployer
{
    public class FunctionsModule : Module
    {
        private readonly ExecutionContext executionContext;
        private readonly Action<ContainerBuilder> registerSpecificDependencies;

        public FunctionsModule(ExecutionContext executionContext, Action<ContainerBuilder> registerSpecificDependencies)
        {
            this.executionContext = executionContext;
            this.registerSpecificDependencies = registerSpecificDependencies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var functions = GetFunctions();
            RegisterThem(functions, builder);
        }

        private void RegisterThem(IEnumerable<IFunction> functions, ContainerBuilder builder)
        {
            functions.ForEach(function => builder.RegisterInstance(function).AsImplementedInterfaces());
        }

        private IEnumerable<IFunction> GetFunctions()
        {
            var builder = new ContainerBuilder();

            var assembly = typeof(Anchor).Assembly;
            var pluginTypes = (from type in assembly.ExportedTypes
                where typeof(DeployerFunction).IsAssignableFrom(type)
                where !type.IsAbstract
                select type).ToList();

            registerSpecificDependencies(builder);
            RegisterDependencies(builder);

            builder.RegisterInstance(executionContext).As<ExecutionContext>();

            pluginTypes.ForEach(t => builder.RegisterType(t));

            var container = builder.Build();

            var query = from n in pluginTypes
                select (DeployerFunction)container.Resolve(n);
            return ((IEnumerable<IFunction>)query).ToImmutableList();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ZipExtractor>().As<IZipExtractor>();
            builder.RegisterType<Downloader>().As<IDownloader>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<Storage>().As<IStorage>();
            builder.RegisterType<BcdInvoker>().As<IBcdInvoker>();
            builder.RegisterType<BootCreator>().As<IBootCreator>();
            builder.RegisterType<DismImageService>().As<IWindowsImageService>();
            builder.RegisterType<AzureDevOpsBuildClient>().As<IAzureDevOpsBuildClient>();
            builder.RegisterInstance(new GitHubClient(new ProductHeaderValue("Deployer"))).As<IGitHubClient>();

            var services = new ServiceCollection();
            services.AddHttpClient();

            services
                .AddRefitClient<IBuildApiClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://dev.azure.com"));

            builder.Populate(services);
        }
    }
}