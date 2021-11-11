using System;
using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.System.Windows;

namespace Deployer.Functions
{
    public class BcdEditCreate : DeployerFunction
    {
        private readonly Func<string, IBcdInvoker> bcdInvokerFactory;

        public BcdEditCreate(Func<string, IBcdInvoker> bcdInvokerFactory)
        {
            this.bcdInvokerFactory = bcdInvokerFactory;
        }

        public async Task Execute(string store, string guid, string args)
        {
            var invoker = bcdInvokerFactory(store);
            var output = await invoker.Invoke($"/enum {{{guid}}}");
            var alreadyExists = output.Contains("{") && output.Contains("}");

            if (alreadyExists)
            {
                return;
            }

            await invoker.Invoke($"/create {{{guid}}} {args}");
        }
    }
}