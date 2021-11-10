using System.Threading.Tasks;

namespace Zafiro.System.Windows
{
    public interface IBcdInvoker
    {
        Task<string> Invoke(string command = "");
    }
}