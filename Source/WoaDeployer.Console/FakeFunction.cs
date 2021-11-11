using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iridio.Common;
using Iridio.Parsing.Model;

namespace Deployer.Console
{
    internal class FakeFunction : IFunction
    {
        public FakeFunction(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Task<object> Invoke(object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parameter> Parameters { get; }
        public Type ReturnType { get; }
    }
}