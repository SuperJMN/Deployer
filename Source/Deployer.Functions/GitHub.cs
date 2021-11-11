using System.Threading.Tasks;
using Deployer.Functions.Core;

namespace Deployer.Functions
{
    public class GitHub : DeployerFunction
    {
        public Task<string> Execute(string owner, string repo, string shaOrBranch = "master")
        {
            return Task.FromResult($"https://github.com/{owner}/{repo}/archive/{shaOrBranch}.zip");
        }
    }
}