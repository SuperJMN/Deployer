using System.Xml;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;

namespace Deployer.Library
{
    public class XmlDeploymentSerializer : IDeployementSerializer
    {
        private readonly IExtendedXmlSerializer serializer;

        public XmlDeploymentSerializer()
        {
            serializer = CreateSerializer();
        }

        public DeployerStore Deserialize(string xmlString)
        {
            return serializer.Deserialize<DeployerStore>(xmlString);
        }

        public string Serialize(DeployerStore store)
        {
            return serializer.Serialize(new XmlWriterSettings
            {
                Indent = true
            }, store);
        }

        private static IExtendedXmlSerializer CreateSerializer()
        {
            return new ConfigurationContainer()
                .UseOptimizedNamespaces()
                .Create();
        }
    }
}