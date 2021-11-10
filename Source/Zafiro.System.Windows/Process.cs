using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Zafiro.Core.Mixins;

namespace Zafiro.System.Windows
{
    // these overloads match the ones in Process.Start to make it a simpler transition for callers
    // see http://msdn.microsoft.com/en-us/library/system.diagnostics.process.start.aspx
    public static class Process
    {
        public static Task<ProcessResults> RunAsync(string fileName)
        {
            return RunAsync(new ProcessStartInfo(fileName));
        }

        public static Task<ProcessResults> RunAsync(string fileName, string arguments)
        {
            return RunAsync(new ProcessStartInfo(fileName, arguments));
        }

        public static Task<ProcessResults> RunAsync(this ProcessStartInfo processStartInfo)
        {
            return RunAsync(processStartInfo, CancellationToken.None);
        }

        public static Task<ProcessResults> RunAsync(ProcessStartInfo processStartInfo, CancellationToken cancellationToken)
        {
            return RunAsync(processStartInfo, s => { }, s => { }, cancellationToken);
        }

        public static Task<ProcessResults> RunAsync(this ProcessStartInfo processStartInfo,
            IObserver<string> onOutputObserver, IObserver<string> onErrorObserver, CancellationToken cancellationToken = default)
        {
            return RunAsync(processStartInfo, s => onOutputObserver?.OnNext(s), s => onErrorObserver?.OnNext(s), cancellationToken);
        }

        public static async Task<ProcessResults> Run(string fileName,
            string args = "", IObserver<string> outputObserver = null, IObserver<string> errorObserver = null, string workingDirectory = "",
            CancellationToken cancellationToken = default)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory
            };

            var logInfo = new
            {
                Command = fileName,
                Arguments = args,
                workingDirectory
            };

            Log.Debug("Running process: {@Info}", logInfo);
            var processResults = await processStartInfo.RunAsync(outputObserver, errorObserver, cancellationToken);
            var resultInfo = new
            {
                processResults.ExitCode,
                OutputOutput = processResults.StandardError.Join(),
                ErrorOutput = processResults.StandardError.Join()
            };

            Log.Debug("End of process. Execution summary: {@Results}", resultInfo);

            return processResults;
        }

        public static async Task<ProcessResults> RunAsync(this ProcessStartInfo processStartInfo, Action<string> onOutput, Action<string> onError,
            CancellationToken cancellationToken)
        {
            // force some settings in the start info so we can capture the output
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            var compiledOutputOutput = new List<string>();
            var compiledErrorOutput = new List<string>();

            var tcs = new TaskCompletionSource<ProcessResults>();

            var process = new global::System.Diagnostics.Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            var standardOutputResults = new TaskCompletionSource<IEnumerable<string>>();
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    onOutput(args.Data);
                    compiledOutputOutput.Add(args.Data);
                }
                else
                {
                    standardOutputResults.SetResult(compiledOutputOutput.AsReadOnly());
                }
            };

            var standardErrorResults = new TaskCompletionSource<IEnumerable<string>>();
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    onError(args.Data);
                    compiledErrorOutput.Add(args.Data);
                }
                else
                {
                    standardErrorResults.SetResult(compiledErrorOutput.AsReadOnly());
                }
            };

            var processStartTime = new TaskCompletionSource<DateTime>();

            process.Exited += async (sender, args) =>
            {
                // Since the Exited event can happen asynchronously to the output and error events, 
                // we await the task results for stdout/stderr to ensure they both closed.  We must await
                // the stdout/stderr tasks instead of just accessing the Result property due to behavior on MacOS.  
                // For more details, see the PR at https://github.com/jamesmanning/RunProcessAsTask/pull/16/
                tcs.TrySetResult(
                    new ProcessResults(
                        process,
                        await processStartTime.Task.ConfigureAwait(false),
                        await standardOutputResults.Task.ConfigureAwait(false),
                        await standardErrorResults.Task.ConfigureAwait(false)
                    )
                );
            };

            using (cancellationToken.Register(
                () =>
                {
                    tcs.TrySetCanceled();
                    try
                    {
                        if (!process.HasExited)
                        {
                            process.Kill();
                        }
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var startTime = DateTime.Now;
                if (process.Start() == false)
                {
                    tcs.TrySetException(new InvalidOperationException("Failed to start process"));
                }
                else
                {
                    try
                    {
                        startTime = process.StartTime;
                    }
                    catch (Exception)
                    {
                        // best effort to try and get a more accurate start time, but if we fail to access StartTime
                        // (for instance, process has already existed), we still have a valid value to use.
                    }

                    processStartTime.SetResult(startTime);

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }

                return await tcs.Task.ConfigureAwait(false);
            }
        }
    }
}