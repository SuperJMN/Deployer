using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Deployer.Compression;
using Deployer.Functions.Core;
using Deployer.Functions.Unzip.Core;
using Zafiro.Network;

namespace Deployer.Functions.Unzip
{
    public class UnzipRoot : UnzipBase
    {
        public UnzipRoot(IZipExtractor extractor, IDownloader downloader, IFileSystem fileSystem, IExecutionContext executionContext) : base(
            extractor,
            downloader, fileSystem, executionContext)
        {
        }

        protected override Task Extract(Stream stream, string finalDir)
        {
            return Extractor.ExtractRoot(stream, finalDir, Progress);
        }
    }
}