using Zafiro.Core;

namespace Deployer.Functions.Core
{
    public class ExecutionContext : IExecutionContext
    {
        public IOperationProgress Operation { get; } = new OperationProgress();
    }
}