using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.PowerShell;

namespace Zafiro.Storage.Windows
{
    public static class PowerShellFacade
    {
        private static bool isExecutionPolicySet;
        private static RunspaceMode runSpace;

        private static async Task<PSDataCollection<PSObject>> Run(Func<PowerShell, Task<PSDataCollection<PSObject>>> task)
        {
            using var ps = CreatePowerShell();
            return await task(ps);
        }

        private static PowerShell CreatePowerShell()
        {
            var initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Bypass;
            using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
            runspace.Open();
            var powerShell = PowerShell.Create();
            powerShell.Runspace = runspace;
            return powerShell;
        }

        public static async Task<Result<PSDataCollection<PSObject>>> WithinPowershell(Action<PowerShell> func)
        {
            var initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Bypass;
            using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
            runspace.Open();
            using var powerShell = PowerShell.Create();
            powerShell.Runspace = runspace;

            func(powerShell);
            var results = await powerShell.InvokeAsync();
            if (powerShell.HadErrors)
            {
                return Result.Failure<PSDataCollection<PSObject>>(string.Join(", ", powerShell.Streams.Error));
            }

            return results;
        }

        public static async Task<PSDataCollection<PSObject>> ExecuteScript(string script)
        {
            var execution = await WithinPowershell(shell => shell.AddScript(script));

            if (execution.IsFailure)
            {
                throw new ApplicationException($"The execution of the script '{script}' failed: {string.Join(", ", execution.Error)}");
            }
            
            return execution.Value;
        }

        public static async Task<PSDataCollection<PSObject>> ExecuteCommand(string commandText, params (string, object)[] parameters)
        {
            var execution = await WithinPowershell(ps =>
            {
                var command = ps.AddCommand(commandText);

                foreach (var (arg, v) in parameters)
                {
                    if (v == null)
                    {
                        command.AddParameter(arg);
                    }
                    else
                    {
                        command.AddParameter(arg, v);
                    }
                }
            });

            if (execution.IsFailure)
            {
                var paramstr = string.Join(",", parameters.Select(tuple => $"{tuple.Item1}={tuple.Item2}"));
                throw new ApplicationException($"The execution of the command '{commandText} failed. Parameters: {paramstr}");
            }

            return execution.Value;
        }
    }
}