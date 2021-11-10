using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.System.Windows;

namespace Deployer.Functions
{
    public class InjectDrivers : DeployerFunction
    {
        private readonly IWindowsImageService imageService;

        public InjectDrivers(IWindowsImageService imageService)
        {
            this.imageService = imageService;
        }

        public async Task Execute(string windowsPath, string origin)
        {
            await imageService.InjectDrivers(origin, windowsPath);
        }
    }
}