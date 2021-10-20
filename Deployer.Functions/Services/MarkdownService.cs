using System.IO.Abstractions;
using System.Threading.Tasks;
using Spectre.Console;

namespace Deployer.Functions.Services
{
    public class MarkdownService : IMarkdownService
    {
        private readonly IFileSystem fileSystem;

        public MarkdownService(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public async Task FromFile(string path)
        {
            var contents = await fileSystem.File.ReadAllTextAsync(path);
            var title = fileSystem.Path.GetFileName(path);
            AnsiConsole.Write(new Panel(new Text(contents, Style.Plain)) { Header = new PanelHeader(title, Justify.Left) });
        }
    }
}