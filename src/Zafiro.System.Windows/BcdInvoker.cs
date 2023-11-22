using System.Threading.Tasks;
using Serilog;

namespace Zafiro.System.Windows
{
    public class BcdInvoker : IBcdInvoker
    {
        private readonly string commonArgs;
        private readonly string bcdEdit;

        public BcdInvoker(string store)
        {
            bcdEdit = ToolPaths.BcdEdit;
            commonArgs = $@"/STORE ""{store}""";
        }

        public async Task<string> Invoke(string command)
        {
            var processResults = await Process.Run(bcdEdit, $@"{commonArgs} {command}");
            var output = string.Join("\n", processResults.StandardOutput);
            var errors = string.Join("\n", processResults.StandardError);
            var result = string.Join(";", output, errors);
            Log.Information($"BCD Edit Command: '{command}'. Result: {result}");
            return result;
        }
    }
}