using Autofac;
using Iridio;
using Iridio.Binding;
using Iridio.Parsing;
using Iridio.Preprocessing;
using Iridio.Runtime;
using FileSystem = System.IO.Abstractions.FileSystem;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace Deployer
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Preprocessor>().As<IPreprocessor>();
            builder.RegisterType<DeployerCore>().AsSelf();
            builder.RegisterType<Binder>().As<IBinder>();
            builder.RegisterType<SourceCodeCompiler>().As<ISourceCodeCompiler>();
            builder.RegisterType<Interpreter>().As<IInterpreter>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<Parser>().As<IParser>();
            builder.RegisterType<RequirementsAnalyzer>().As<IRequirementsAnalyzer>();
        }
    }
}