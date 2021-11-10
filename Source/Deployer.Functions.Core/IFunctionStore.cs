using System.Collections.Generic;
using Iridio.Common;

namespace Deployer.Functions.Core
{
    public interface IFunctionStore
    {
        IEnumerable<IFunction> Functions { get; }
    }
}