using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;

namespace Deployer.Library
{
    public class XmlDeploymentReader : IDeployementReader
    {
        private readonly string xmlString;
        private readonly IExtendedXmlSerializer serializer;
        private readonly DeployerStore story;


        public XmlDeploymentReader()
        {
            serializer = CreateSerializer();
        }

        public DeployerStore Read(string xmlString)
        {
            return serializer.Deserialize<DeployerStore>(xmlString);
        }

        public string Write(DeployerStore store)
        {
            return serializer.Serialize(store);
        }

        private static IExtendedXmlSerializer CreateSerializer()
        {
            return new ConfigurationContainer()
                .Type<Device>().EnableReferences(x => x.Id)
                .Type<Deployment>().Member(x => x.Id).Attribute()
                .UseOptimizedNamespaces()
                .Create();
        }
    }
}