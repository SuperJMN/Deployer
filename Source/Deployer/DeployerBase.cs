using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Iridio.Runtime;

namespace Deployer
{
    public abstract class DeployerBase : IDeployer
    {
        private readonly DeployerCore core;

        protected DeployerBase()
        {
            var builder = new ContainerBuilder();
            RegisterDependencies(builder);
            core = builder.Build().Resolve<DeployerCore>();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterModule<InfrastructureModule>();
            RegisterFunctions(builder);
        }

        protected abstract void RegisterFunctions(ContainerBuilder builder);

        public Task<Result<ExecutionSummary, IridioError>> Run(string path, IDictionary<string, object> initialState)
        {
            return core.Run(path, initialState);
        }

        public Result<BuildArtifact, IridioError> Check(string path)
        {
            return core.Check(path);
        }

        public IObservable<string> Messages => core.Messages;
        public ExecutionContext ExecutionContext { get; } = new();
    }
}