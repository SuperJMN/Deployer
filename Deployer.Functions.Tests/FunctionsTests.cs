using Deployer.Functions.Core;
using FluentAssertions;
using Xunit;

namespace Deployer.Functions.Tests
{
    public class FunctionsTests
    {
        [Fact]
        public void Test()
        {
            var functionStore =
                new PluginFunctionStore(
                    "C:\\Users\\JMN\\source\\repos\\WoaDeployer.Console\\Deployer.Functions\\bin\\Debug\\net5.0\\Deployer.Functions.dll",
                    new ExecutionContext());
            var functions = functionStore.Functions;
            functions.Should().NotBeEmpty();
        }
    }
}