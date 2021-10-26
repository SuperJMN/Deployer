using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Deployer.Gui.ViewModels
{
    public class RequirementFiller
    {
        public async Task<Result<Dictionary<string, object>>> Fill(IEnumerable<Requirement> getRequirements)
        {
            var dictionary = new Dictionary<string, object>
            {
                ["Disk"] = 4,
                ["DeploymentSize"] = 16D,
                ["WimFileIndex"] = 1,
                ["WimFilePath"] = "J:\\sources\\install.wim"
            };

            return dictionary;
        }
    }
}