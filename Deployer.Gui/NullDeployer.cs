using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Iridio.Runtime;

namespace Deployer.Gui
{
    public class NullDeployer : IDeployer
    {
        public Task<Result<ExecutionSummary, IridioError>> Run(string path, IDictionary<string, object> initialState)
        {
            throw new NotImplementedException();
        }

        public Result<BuildArtifact, IridioError> Check(string path)
        {
            throw new NotImplementedException();
        }

        public IObservable<string> Messages => new Subject<string>();
        public ExecutionContext ExecutionContext => new();
    }
}