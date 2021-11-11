namespace Deployer.Library
{
    public interface IDeployementSerializer
    {
        DeployerStore Deserialize(string xmlString);
        string Serialize(DeployerStore store);
    }
}