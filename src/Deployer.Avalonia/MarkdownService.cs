using System.Threading.Tasks;
using Deployer.Functions.Services;

namespace Deployer.Avalonia
{
    public class MarkdownService : IMarkdownService
    {
        public Task FromFile(string path)
        {
            return Task.CompletedTask;
        }
    }
}