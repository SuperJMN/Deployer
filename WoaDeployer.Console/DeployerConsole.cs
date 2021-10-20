using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Iridio.Common;
using Iridio.Runtime;
using MoreLinq;
using Serilog;
using Spectre.Console;

namespace Deployer.Console
{
    public class DeployerConsole
    {
        private readonly IDeployer deployer;
        private readonly IFileSystem fileSystem;
        private readonly IExecutionContext executionContext;
        private readonly IEnumerable<IFunction> functions;

        public DeployerConsole(IDeployer deployer, IFileSystem fileSystem, IExecutionContext executionContext, IEnumerable<IFunction> functions)
        {
            this.deployer = deployer;
            this.fileSystem = fileSystem;
            this.executionContext = executionContext;
            this.functions = functions;
        }

        public async Task<int> Run(string[] args)
        {
            System.Console.OutputEncoding = Encoding.UTF8;

            Log.Information("Executing console");
            var root = RootCommand();
            return await root.InvokeAsync(args);
        }

        private RootCommand RootCommand()
        {
            var root = new RootCommand("Deploys smoothly");
            root.AddCommand(ScriptCommand());
            root.AddCommand(ListCommand());
            return root;
        }

        private Command ScriptCommand()
        {
            var root = new Command("script");
            root.AddCommand(RunCommand());
            root.AddCommand(CheckCommand());

            return root;
        }

        private Command RunCommand()
        {
            var run = new Command("run")
            {
                Handler = CommandHandler.Create(async (IFileInfo script, IEnumerable<string> set, InvocationContext ctx) =>
                    {
                        await RunScript(set, script, ctx);
                    })
            };
            run.Add(new Argument<IFileInfo>("script", argResult =>
            {
                var fileName = argResult.Tokens.First().Value;
                var fromFileName = fileSystem.FileInfo.FromFileName(fileName);
                if (!fromFileName.Exists)
                {
                    argResult.ErrorMessage = $"The file doesn't exist {fileName}";
                }

                return fromFileName;
            }));

            run.Add(new Option<string>("--set", "Set a variable. For instance --set a=Disk=3") { Arity = ArgumentArity.ZeroOrMore });

            return run;
        }

        private async Task RunScript(IEnumerable<string> assignments, IFileInfo script, InvocationContext invocationContext)
        {
            var requirements = Requirements.FromSource(await fileSystem.File.ReadAllTextAsync(script.FullName));
            var initialState = InitialState.Create(assignments, requirements);

            await AnsiConsole
                .Progress()
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(), new PercentageColumn(),
                    new RemainingTimeColumn(),
                    new SpinnerColumn(Spinner.Known.BouncingBall))
                .StartAsync(async ctx =>
                {
                    using var progressUpdater = new ProgressUpdater(deployer, executionContext, ctx);
                    return await deployer
                        .Run(script.FullName, initialState)
                        .Tap(() => AnsiConsole.MarkupLine("[bold green]Success![/]"))
                        .OnFailure(error => HandleFailure(invocationContext, error));
                });
        }

        private static void HandleFailure(InvocationContext invocationContext, IridioError e)
        {
            Log.Error("Run failed {@Errors}", e.Errors.Select(i => new { i.Message }));
            var b = new StringBuilder();
            b.Append("[red]Deployment failed[/]\n");
            foreach (var errorItem in e.Errors)
            {
                var message = errorItem
                    .SourceUnit
                    .Match(sc => $"[silver]{Markup.Escape(sc.ToString())}[/]:\n\t[white]{Markup.Escape(errorItem.Message)}[/]",
                        () => "[white]{errorItem.Message}[/]");
                b.AppendLine(message);
            }

            AnsiConsole.MarkupLine(b.ToString());
            invocationContext.ExitCode = 1;
        }

        private Result CheckFile(string script)
        {
            return Result.SuccessIf(() => fileSystem.File.Exists(script), "File doesn't exist");
        }

        private Command CheckCommand()
        {
            var run = new Command("check")
            {
                Handler = CommandHandler.Create((string script, InvocationContext ctx) =>
                    CheckFile(script)
                        .Map(() => deployer.Check(script)
                            .Tap(() => System.Console.WriteLine("The file is OK"))
                            .OnFailure(Beautify))
                        .OnFailure(() => ctx.ExitCode = 1))
            };
            run.Add(new Argument("script"));
            return run;
        }

        private Command ListCommand()
        {
            var funcsCommand = new Command("functions")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var table = new Table();

                    table.RoundedBorder();


                    table.AddColumn(new TableColumn("Type").Centered());
                    table.AddColumn(new TableColumn("Name"));
                    table.AddColumn(new TableColumn("Parameters"));
                    foreach (var func in functions.OrderBy(f => f.Name))
                    {
                        var inner = new Table();
                        inner.AddColumn(new TableColumn("Type").Centered());
                        inner.AddColumn(new TableColumn("Name"));

                        foreach (var p in func.Parameters)
                        {
                            inner.AddRow(new Markup(p.Type.Name), new Markup(p.Name));
                        }

                        table.AddRow(new Markup(func.ReturnType.Name), new Markup(func.Name), inner);
                    }

                    AnsiConsole.Write(table);
                })
            };

            var command = new Command("list");
            command.AddCommand(funcsCommand);
            return command;
        }

        private static void Beautify(IridioError error)
        {
            error.Errors.Select((e, i) => new { item = e, index = i + 1 }).ForEach(err =>
            {
                err.item.SourceUnit.Execute(unit =>
                {
                    var locationInfo = Wrap(unit.ToString(), 50);
                    AnsiConsole.Write($"[gray]{err.index}) {locationInfo}[/]\t");
                });
                AnsiConsole.WriteLine("[darkred]" + err.item.Message + "[/]");
            });

            AnsiConsole.Write($"\t[rgb(216,66,66)]{error.Errors.Count} error/s[/]");
        }

        private static string Wrap(string str, int i)
        {
            var final = new string(str.Take(i).ToArray());

            if (str.Length > i)
            {
                final += "...";
            }

            return final;
        }
    }
}