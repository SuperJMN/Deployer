using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Deployer.Functions.Filesystem;
using Zafiro.Storage;

namespace Deployer.Functions;

// ReSharper disable once UnusedType.Global
public class ApplyRpi3GptHack : DeployerFunction
{
    private readonly IStorage storage;

    public ApplyRpi3GptHack(IStorage storage)
    {
        this.storage = storage;
    }

    public async Task Execute(int diskNumber)
    {
        var disk = await storage.GetDisk(diskNumber);
        disk.Tap(Raspberrypi3Hack.Apply);
    }
}