using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Zafiro.Storage.Misc
{
    public static class FileSystemOperations
    {
        public static void EnsureDirectoryExists(this IFileSystem fileSystem, string directoryPath)
        {
            if (!fileSystem.Directory.Exists(directoryPath))
            {
                CreateDirectory(fileSystem, directoryPath);
            }
        }

        public static IEnumerable<string> QueryDirectory(this IFileSystem fileSystem, string root, Func<string, bool> selector = null)
        {
            var allDirectories = fileSystem.Directory.EnumerateDirectories(root, "*", SearchOption.AllDirectories);

            if (selector != null)
            {
                allDirectories = allDirectories.Where(selector);
            }

            return allDirectories;
        }

        public static async Task Copy(this IFileSystem fileSystem, string source, string destination, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (destination.EndsWith(char.ToString(fileSystem.Path.DirectorySeparatorChar)))
            {
                destination = fileSystem.Path.Combine(destination, fileSystem.Path.GetFileName(source));
            }

            var dir = fileSystem.Path.GetDirectoryName(destination);

            if (dir == null)
            {
                throw new InvalidOperationException("The directory name could be retrieved");
            }

            EnsureDirectoryExists(fileSystem, dir);

            var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var bufferSize = 4096;

            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize,
                fileOptions))
            using (var destinationStream = new FileStream(destination, FileMode.Create, FileAccess.Write,
                FileShare.None, bufferSize, fileOptions))
            {
                await sourceStream.CopyToAsync(destinationStream, bufferSize).ConfigureAwait(false);
            }
        }

        public static Task CopyDirectory(this IFileSystem fileSystem, string source, string destination, string fileSearchPattern = null,
            CancellationToken cancellationToken = default)
        {
            return CopyDirectory(fileSystem, fileSystem.DirectoryInfo.FromDirectoryName(source),
                fileSystem.DirectoryInfo.FromDirectoryName(destination), fileSearchPattern, false,
                cancellationToken);
        }

        public static void CreateDirectory(IFileSystem fileSystem, string destPath)
        {
            if (!IsExistingPath(fileSystem, destPath))
            {
                fileSystem.Directory.CreateDirectory(destPath);
            }
        }

        private static async Task CopyDirectory(this IFileSystem fileSystem, IDirectoryInfo source, IDirectoryInfo destination,
            string fileSearchPattern,
            bool skipEmptyDirectories, CancellationToken cancellationToken)
        {
            try
            {
                var files = fileSearchPattern == null ? source.GetFiles() : source.GetFiles(fileSearchPattern);

                foreach (var dir in source.GetDirectories().Where(d => !d.Attributes.HasFlag(FileAttributes.Hidden)))
                {
                    var subDirFiles = fileSearchPattern == null ? dir.GetFiles() : dir.GetFiles(fileSearchPattern);
                    if (!subDirFiles.Any() && skipEmptyDirectories)
                    {
                        continue;
                    }

                    // TODO: This is a workaround for FileInfo.CreateSubdirectory(...)
                    // See https://github.com/dotnet/runtime/issues/49284
                    var subdir = Path.Combine(destination.FullName, dir.Name);
                    var subdirectory = new DirectoryInfoWrapper(fileSystem, Directory.CreateDirectory(subdir));
                    await CopyDirectory(fileSystem, dir, subdirectory, fileSearchPattern, skipEmptyDirectories, cancellationToken);
                }

                foreach (var file in files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)))
                {
                    var destFileName = fileSystem.Path.Combine(destination.FullName, file.Name);
                    await Copy(fileSystem, file.FullName, destFileName, cancellationToken);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Warning(e, "Unauthorized folder {Folder}", source);
            }
        }

        private static bool IsExistingPath(this IFileSystem fileSystem, string path)
        {
            var isExistingPath = fileSystem.File.Exists(path) || fileSystem.Directory.Exists(path);
            var status = isExistingPath ? "exists" : "does not exist";

            Log.Verbose($"Checking path: {{Path}} {status}", path);

            return isExistingPath;
        }
    }
}