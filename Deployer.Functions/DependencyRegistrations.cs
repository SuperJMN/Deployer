using System;
using System.IO.Abstractions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deployer.Compression;
using Deployer.Functions.Services;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Refit;
using Zafiro.Core.FileSystem;
using Zafiro.Network;
using Zafiro.Storage;
using Zafiro.Storage.Windows;
using Zafiro.System.Windows;
using Zafiro.System.Windows.Dism;
using Zafiro.Tools.AzureDevOps;

namespace Deployer.Functions
{
    public class DependencyRegistrations : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ZipExtractor>().As<IZipExtractor>();
            builder.RegisterType<Downloader>().As<IDownloader>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<FileSystemOperations>().As<IFileSystemOperations>();
            builder.RegisterType<ZipExtractor>().As<IZipExtractor>();
            builder.RegisterType<Storage>().As<IStorage>();
            builder.RegisterType<BcdInvoker>().As<IBcdInvoker>();
            builder.RegisterType<BootCreator>().As<IBootCreator>();
            builder.RegisterType<MarkdownService>().As<IMarkdownService>();
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