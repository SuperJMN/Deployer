using Zafiro.Core;

namespace Deployer.Functions.Core
{
    public interface IExecutionContext
    {
        IOperationProgress Operation { get; }
    }
}