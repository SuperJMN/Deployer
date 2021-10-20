using System.IO;
using System.Threading.Tasks;
using Zafiro.Core;

namespace Deployer.Compression
{
    public interface IZipExtractor
    {
        Task Extract(Stream stream, string destination, IOperationProgress progressObserver = null);

        Task ExtractRoot(Stream stream, string destination,
            IOperationProgress progressObserver = null);
    }
}