using System;
using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.System.Windows;

namespace Deployer.Functions
{
    public class BcdEdit : DeployerFunction
    {
        private readonly Func<string, IBcdInvoker> bcdInvokerFactory;

        public BcdEdit(Func<string, IBcdInvoker> bcdInvokerFactory)
        {
            this.bcdInvokerFactory = bcdInvokerFactory;
        }

        public Task Execute(string store, string command)
        {
            return bcdInvokerFactory(store).Invoke(command);
        }
    }
}