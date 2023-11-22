using System.Threading.Tasks;
using Deployer.Functions.Core;
using Deployer.Functions.Services;

namespace Deployer.Functions
{
    public class DisplayMarkdown : DeployerFunction
    {
        private readonly IMarkdownService markdownService;

        public DisplayMarkdown(IMarkdownService markdownService)
        {
            this.markdownService = markdownService;
        }

        public Task Execute(string path)
        {
            return markdownService.FromFile(path);
        }
    }
}