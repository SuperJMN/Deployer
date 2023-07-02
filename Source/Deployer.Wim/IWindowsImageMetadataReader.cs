using CSharpFunctionalExtensions;

namespace Deployer.Wim
{
    public interface IWindowsImageMetadataReader
    {
        Result<XmlWindowsImageMetadata> Load(string path);
    }
}