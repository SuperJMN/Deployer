using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Deployer.Gui
{
    public interface IFeedInstaller
    {
        Task<Result> Install();
    }
}