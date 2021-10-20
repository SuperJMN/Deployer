using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.System.Windows;

namespace Deployer.Functions
{
    public class MakeWindowsBootable : DeployerFunction
    {
        private readonly IBootCreator bootCreator;

        public MakeWindowsBootable(IBootCreator bootCreator)
        {
            this.bootCreator = bootCreator;
        }

        public Task Execute(string systemRoot, string windowsRoot)
        {
            return bootCreator.MakeBootable(systemRoot, windowsRoot);
        }
    }
}