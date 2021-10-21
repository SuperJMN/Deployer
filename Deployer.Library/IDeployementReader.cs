namespace Deployer.Library
{
    public interface IDeployementReader
    {
        DeployerStore Read(string xmlString);
        string Write(DeployerStore store);
    }
}