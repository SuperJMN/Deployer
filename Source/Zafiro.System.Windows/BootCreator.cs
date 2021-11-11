using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Serilog;

namespace Zafiro.System.Windows
{
    public class BootCreator : IBootCreator
    {
        private readonly Func<string, IBcdInvoker> bcdInvokerFactory;
        private readonly IFileSystem fileSystem;

        public BootCreator(Func<string, IBcdInvoker> bcdInvokerFactory, IFileSystem fileSystem)
        {
            this.bcdInvokerFactory = bcdInvokerFactory;
            this.fileSystem = fileSystem;
        }

        public async Task MakeBootable(string systemRoot, string windowsPath)
        {
            Log.Verbose("Making Windows installation bootable...");

            var bcdInvoker = bcdInvokerFactory(CombineRelativeBcdPath(systemRoot));

            await Process.Run(ToolPaths.BcdBoot, $@"{windowsPath} /f UEFI /s {systemRoot} /l en-us");
            await bcdInvoker.Invoke("/set {default} testsigning on");
            await bcdInvoker.Invoke("/set {default} nointegritychecks on");
        }

        private string CombineRelativeBcdPath(string root)
        {
            return fileSystem.Path.Combine(root, "EFI", "Microsoft", "Boot", "BCD");
        }
    }
}