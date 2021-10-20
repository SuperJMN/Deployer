using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;

namespace Deployer.Functions
{
    public class Print : DeployerFunction
    {
        public Task<Result> Execute(string message)
        {
            Console.WriteLine(message);
            return Task.FromResult(Result.Success());
        }
    }
}