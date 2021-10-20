using System;
using System.Reactive.Subjects;
using Zafiro.Core;
using Zafiro.Core.ProgressReporting;

namespace Deployer.Functions.Core
{
    public class OperationProgress : IOperationProgress
    {
        private readonly Subject<Progress> progress;

        public OperationProgress()
        {
            progress = new Subject<Progress>();
        }

        public void Send(Progress current)
        {
            progress.OnNext(current);
        }

        public IObservable<Progress> Progress => progress;
    }
}