using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Deployer.Compression;
using Deployer.Functions.Core;
using Serilog;
using Zafiro.Core;
using Zafiro.Network;

namespace Deployer.Functions.Unzip.Core
{
    public abstract class UnzipBase : DeployerFunction
    {
        protected readonly IZipExtractor Extractor;
        private readonly IDownloader downloader;
        protected readonly IOperationProgress Progress;
        private readonly IFileSystem fileSystem;
        private readonly ExecutionContext executionContext;

        public UnzipBase(IZipExtractor extractor, IDownloader downloader, IFileSystem fileSystem, ExecutionContext executionContext)
        {
            Extractor = extractor;
            this.downloader = downloader;
            this.fileSystem = fileSystem;
            this.executionContext = executionContext;
        }

        public async Task Execute(string url, string destination, string artifactName = null)
        {
            var finalDir = Path.Combine(destination, artifactName ?? Path.GetFileNameWithoutExtension(GetFileName(url)));

            if (fileSystem.Directory.Exists(finalDir))
            {
                Log.Warning("{Url} already downloaded. Skipping download.", url);
                return;
            }
            
            using (var stream = await downloader.GetStream(new Uri(url), executionContext.Operation))
            {
                await Extract(stream, finalDir);
            }
        }

        protected abstract Task Extract(Stream stream, string finalDir);

        private static string GetFileName(string urlString)
        {
            if (Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
            {
                var filename = Path.GetFileName(uri.LocalPath);
                return filename;
            }

            if (File.Exists(urlString))
            {
                return Path.GetFileName(urlString);
            }

            throw new InvalidOperationException($"Unsupported URL: {urlString}");
        }
    }
}