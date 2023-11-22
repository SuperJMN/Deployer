using Zafiro.Core;

namespace Deployer.Functions.Core
{
    public class ExecutionContext
    {
        public IOperationProgress Operation { get; } = new OperationProgress();
    }
}