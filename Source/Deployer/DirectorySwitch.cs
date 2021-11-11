using System;
using System.IO.Abstractions;
using CSharpFunctionalExtensions;
using Serilog;

namespace Deployer
{
    public class DirectorySwitch : IDisposable
    {
        private readonly IFileSystem fileSystem;
        private readonly Maybe<string> oldDirectory;

        public DirectorySwitch(IFileSystem fileSystem, string directory)
        {
            if (directory.Trim() == "")
            {
                return;
            }

            this.fileSystem = fileSystem;
            Log.Debug("Switching to " + directory);
            this.oldDirectory = fileSystem.Directory.GetCurrentDirectory();
            fileSystem.Directory.SetCurrentDirectory(directory);
        }

        public void Dispose()
        {
            oldDirectory.Execute(old =>
            {
                Log.Debug("Returning to " + this.oldDirectory);
                fileSystem.Directory.SetCurrentDirectory(old);
            });
        }
    }
}