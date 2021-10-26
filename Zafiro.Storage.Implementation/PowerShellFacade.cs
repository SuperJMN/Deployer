﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Zafiro.Storage.Windows
{
    public static class PowerShellFacade
    {
        private static bool isExecutionPolicySet;

        private static async Task<PSDataCollection<PSObject>> Run(Func<PowerShell, Task<PSDataCollection<PSObject>>> task)
        {
            using var ps = PowerShell.Create();
            SetExecutionPolicy(ps);
            return await task(ps);
        }

        private static void SetExecutionPolicy(PowerShell powerShell)
        {
            if (isExecutionPolicySet)
            {
                return;
            }

            powerShell.AddScript("Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process");
            powerShell.Invoke();
            isExecutionPolicySet = true;
        }

        public static async Task<PSDataCollection<PSObject>> ExecuteScript(string script)
        {
            return await Run(async ps =>
            {
                ps.AddScript(script, true);
                var results = await Task.Factory.FromAsync(ps.BeginInvoke(), ps.EndInvoke);

                if (ps.HadErrors)
                {
                    throw new ApplicationException($"The execution of the script '{script}' failed: {string.Join(", ", ps.Streams.Error)}");
                }

                return results;
            });
        }

        public static async Task<PSDataCollection<PSObject>> ExecuteCommand(string commandText, params (string, object)[] parameters)
        {
            return await Run(async ps =>
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

                var psDataCollection = await Task.Factory.FromAsync(ps.BeginInvoke(), ps.EndInvoke);

                if (ps.HadErrors)
                {
                    var paramstr = string.Join(",", parameters.Select(tuple => $"{tuple.Item1}={tuple.Item2}"));
                    throw new ApplicationException($"The execution of the command '{commandText} failed. Parameters: {paramstr}");
                }

                return psDataCollection;
            });
        }

        public static async Task<PSDataCollection<PSObject>> ExecuteCommand(this PowerShell ps, string commandText, IEnumerable<object> arguments,
            params (string, object)[] parameters)
        {
            ps.Commands.Clear();

            var command = ps.AddCommand(commandText);

            foreach (var (arg, v) in parameters)
            {
                command.AddParameter(arg, v);
            }

            var psDataCollection = await Task.Factory.FromAsync(ps.BeginInvoke(), ps.EndInvoke);

            if (ps.HadErrors)
            {
                throw new ApplicationException($"The execution of the command '{commandText}' failed");
            }

            return psDataCollection;
        }
    }
}