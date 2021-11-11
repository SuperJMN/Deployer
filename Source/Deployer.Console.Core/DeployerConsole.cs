using Autofac;

namespace Deployer.Console.Core
{
    public class DeployerConsole : DeployerBase
    {
        protected override void RegisterFunctions(ContainerBuilder builder)
        {
            var functionsModule = new FunctionsModule(ExecutionContext, c => c.RegisterModule<ConsoleModule>());
            builder.RegisterModule(functionsModule);
        }
    }
}