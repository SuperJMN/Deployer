using System;
using System.Reactive.Disposables;
using CSharpFunctionalExtensions;
using Spectre.Console;
using Zafiro.Core;
using Zafiro.Core.ProgressReporting;

namespace Deployer.Console
{
    internal class ProgressUpdater : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private Maybe<ProgressTask> currentTask = Maybe<ProgressTask>.None;

        public ProgressUpdater(IDeployer deployer, ProgressContext ctx)
        {
            deployer.Messages
                .Subscribe(s =>
                {
                    currentTask.Execute(p =>
                    {
                        p.StopTask();
                        p.Value = 1;
                    });

                    currentTask = Maybe.From(ctx.AddTask(s, true, 1));

                    currentTask.Execute(p => { p.IsIndeterminate = true; });
                }).DisposeWith(disposable);

            deployer.ExecutionContext.Operation.Progress.Subscribe(progress =>
            {
                switch (progress)
                {
                    case Done:
                        currentTask.Execute(p =>
                        {
                            p.Value = 1;
                            p.StopTask();
                        });
                        break;
                    case AbsoluteProgress<ulong> absoluteProgress:
                        currentTask.Execute(p =>
                        {
                            p.IsIndeterminate = true;
                            p.Value = absoluteProgress.Value;
                        });

                        break;
                    case Percentage percentage:

                        currentTask.Execute(p =>
                        {
                            p.Value = percentage.Value;
                            p.IsIndeterminate = false;
                        });

                        break;
                    case Unknown:
                        currentTask.Execute(p => { p.IsIndeterminate = true; });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(progress));
                }
            }).DisposeWith(disposable);
        }

        public void Dispose()
        {
            disposable.Dispose();
            currentTask.Execute(p =>
            {
                p.Value = 1;
                p.StopTask();
            });
        }
    }
}