using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Zafiro.Storage
{
    public interface IStorage
    {
        Task<IList<IDisk>> GetDisks();
        Task<Result<IDisk>> GetDisk(int n);
    }
}