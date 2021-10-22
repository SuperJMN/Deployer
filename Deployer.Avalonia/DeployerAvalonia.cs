using Autofac;

namespace Deployer.Avalonia
{
    public class DeployerAvalonia : DeployerBase
    {
        protected override void RegisterFunctions(ContainerBuilder builder)
        {
            var functionsModule = new FunctionsModule(ExecutionContext, c => c.RegisterModule<AvaloniaModule>());
            builder.RegisterModule(functionsModule);
        }
    }
}