using System.Threading.Tasks;

namespace Zafiro.System.Windows
{
    public interface IBootCreator
    {
        Task MakeBootable(string systemRoot, string windowsPath);
    }
}