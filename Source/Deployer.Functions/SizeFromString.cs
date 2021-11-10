using System.Threading.Tasks;
using ByteSizeLib;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;

namespace Deployer.Functions
{
    public class SizeFromString : DeployerFunction
    {
        public Task<Result<double>> Execute(string sizeString)
        {
            return Task.FromResult(ByteSize.TryParse(sizeString, out var v)
                ? Result.Success(v.Bytes)
                : Result.Failure<double>($"Could not parse {sizeString} to a byte size"));
        }
    }
}