using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Runtime;

namespace Deployer
{
    public interface IDeployer
    {
        Task<Result<ExecutionSummary, IridioError>> Run(string path, IDictionary<string, object> initialState);
        Result<BuildArtifact, IridioError> Check(string path);
        IObservable<string> Messages { get; }
    }
}