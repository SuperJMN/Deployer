﻿namespace Deployer.Tests
{
    //public class DeployerConsoleTests
    //{
    //    [Fact]
    //    public async Task Existing_script_should_be_deployed()
    //    {
    //        var deployer = Mock.Of<IDeployer>(d => d.Messages == new Subject<string>());

    //        IDictionary<string, MockFileData> info = new Dictionary<string, MockFileData>
    //        {
    //            ["test.txt"] = MockFileData.NullObject
    //        };

    //        var sut = new DeployerCore(deployer, new MockFileSystem(info), new ExecutionContext(), new List<IFunction>());
    //        var result = await sut.Run(new[] { "script", "run", "test.txt" });
    //        result.Should().Be(0);
    //        Mock.Get(deployer).Verify(d => d.Run("test.txt", new Dictionary<string, object>()), Times.Once);
    //    }

    //    [Fact]
    //    public async Task Non_existing_script_should_be_deployed()
    //    {
    //        var deployer = Mock.Of<IDeployer>(d => d.Messages == new Subject<string>());

    //        var sut = new DeployerConsole(deployer, new MockFileSystem(), new ExecutionContext(), new List<IFunction>());
    //        var result = await sut.Run(new[] { "script", "run", "test.txt" });
    //        result.Should().NotBe(0);
    //        Mock.Get(deployer).Verify(d => d.Run("test.txt", new Dictionary<string, object>()), Times.Never);
    //    }
    //}
}