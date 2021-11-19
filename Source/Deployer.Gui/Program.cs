using Avalonia;
using Avalonia.ReactiveUI;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using Zafiro.Tools.AzureDevOps.BuildsModel;

namespace Deployer.Gui
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            ConfigureLogging();

            try
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "The application has encountered an unrecoverable error. The application has been shut down");
                throw;
            }
        }

        private static void ConfigureLogging()
        {
            var logsFolderPath = GetLogsFolderPath();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logsFolderPath, "Log.txt"), rollingInterval: RollingInterval.Day)
                .MinimumLevel.Verbose()
                .CreateLogger();

            Log.Information("Log path set to {Path}", logsFolderPath);
        }
        
        private static string GetLogsFolderPath()
        {
            return Path.Combine(Path.GetTempPath(), "Deployer", "Logs");
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
