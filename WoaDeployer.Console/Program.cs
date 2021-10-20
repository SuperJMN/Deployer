using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Deployer.Functions;
using Deployer.Functions.Core;
using Iridio;
using Iridio.Binding;
using Iridio.Parsing;
using Iridio.Preprocessing;
using Iridio.Runtime;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;
using FileSystem = System.IO.Abstractions.FileSystem;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace Deployer.Console
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            ConfigureLogging();

            var console = GetDeployerConsole();
            await console.Run(args);
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Logs\\log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.SpectreConsole("{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}", LogEventLevel.Information)
                .MinimumLevel.Verbose()
                .CreateLogger();
        }

        private static DeployerConsole GetDeployerConsole()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Preprocessor>().As<IPreprocessor>();
            builder.RegisterType<DeployerCore>().As<IDeployer>();
            builder.RegisterType<Binder>().As<IBinder>();
            builder.RegisterType<SourceCodeCompiler>().As<ISourceCodeCompiler>();
            builder.RegisterType<Interpreter>().As<IInterpreter>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<Parser>().As<IParser>();
            builder.RegisterType<RequirementsAnalyzer>().As<IRequirementsAnalyzer>();
            var executionContext = new ExecutionContext();

            var funcs = new FunctionStore(executionContext).Functions.ToList();
            funcs.ForEach(function => builder.RegisterInstance(function).AsImplementedInterfaces());
            builder.RegisterInstance(executionContext).As<IExecutionContext>();
            builder.RegisterType<DeployerConsole>();
            var container = builder.Build();
            var console = container.Resolve<DeployerConsole>();
            return console;
        }
    }
}
