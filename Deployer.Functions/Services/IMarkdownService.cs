using System.Threading.Tasks;

namespace Deployer.Functions.Services
{
    public interface IMarkdownService
    {
        Task FromFile(string path);
    }
}