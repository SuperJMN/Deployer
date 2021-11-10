using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;

namespace Deployer.Functions
{
    public class Fail : DeployerFunction
    {
        public Task<Result> Execute(string reason)
        {
            return Task.FromResult(Result.Failure(reason));
        }
    }
}
