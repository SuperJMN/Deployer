using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Autofac;
using CSharpFunctionalExtensions;
using Iridio.Common;
using Module = Autofac.Module;

namespace Deployer.Functions.Core
{
    public class PluginFunctionStore : IFunctionStore
    {
        private readonly ExecutionContext executionContext;
        private readonly string pluginLocation;

        public PluginFunctionStore(string pluginLocation, ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
            this.pluginLocation = pluginLocation;
        }

        public IEnumerable<IFunction> Functions
        {
            get
            {
                var builder = new ContainerBuilder();
                var assembly = LoadAssembly(pluginLocation);
                var pluginTypes = (from type in assembly.ExportedTypes
                    where typeof(DeployerFunction).IsAssignableFrom(type)
                    where !type.IsAbstract
                    select type).ToList();

                RegisterDependencies(assembly, builder);

                builder.RegisterInstance(executionContext).As<ExecutionContext>();

                pluginTypes.ForEach(t => builder.RegisterType(t));

                var container = builder.Build();

                var query = from n in pluginTypes
                    select (DeployerFunction)container.Resolve(n);
                return ((IEnumerable<IFunction>)query).ToImmutableList();
            }
        }

        private static void RegisterDependencies(Assembly assembly, ContainerBuilder container)
        {
            assembly.ExportedTypes.ToList().TryFirst(t =>
                {
                    var isModule = typeof(Module).IsAssignableFrom(t);
                    var isNameMatch = t.Name == "DependencyRegistrations";
                    return isNameMatch && isModule;
                }).Map(type => (Module)Activator.CreateInstance(type))
                .Execute(type => container.RegisterModule(type));
        }

        private static Assembly LoadAssembly(string pluginLocation)
        {
            var pluginCatalog = new PluginLoadContext(pluginLocation);
            return pluginCatalog.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginLocation));
        }
    }
}