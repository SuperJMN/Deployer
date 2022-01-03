using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Filesystem;
using Xunit;
using Zafiro.Storage.Windows;

namespace Deployer.Functions.Tests
{
    public class FunctionsTests
    {
        [Fact(Skip = "This is dangerous. Think before executing it")]
        public async Task Test()
        {
            var storage = new Storage();
            var disk = await storage.GetDisk(4);
            disk.Tap(d => Raspberrypi3Hack.Apply(d));
        }
    }
}