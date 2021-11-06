using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Deployer.Compression;
using Zafiro.Network;

namespace Deployer.Gui
{
    public class FeedInstaller : IFeedInstaller
    {
        private readonly IDeployer deployer;
        private readonly IZipExtractor zipExtractor;
        private IFileSystem fileSystem;
        private IDownloader downloader;

        public FeedInstaller(IDeployer deployer)
        {
            this.deployer = deployer;
        }

        public async Task Install()
        {
            await deployer.Run("Bootstrap.txt", new Dictionary<string, object>());
        }
    }
}