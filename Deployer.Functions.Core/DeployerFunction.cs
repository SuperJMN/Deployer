using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Iridio.Common;
using Iridio.Common.Utils;
using Iridio.Parsing.Model;

namespace Deployer.Functions.Core
{
    public class DeployerFunction : IFunction
    {
        private readonly MethodInfo execute;

        public DeployerFunction()
        {
            execute = GetType().GetMethod("Execute");
        }

        public string Name => GetType().Name;

        public Task<object> Invoke(object[] parameters)
        {
            return execute.InvokeTask(this, parameters);
        }

        public IEnumerable<Parameter> Parameters => execute.GetParameters().Select(x => new Parameter(x.Name, x.ParameterType));
        public Type ReturnType => execute.ReturnType;

        public override string ToString()
        {
            return Name;
        }
    }
}