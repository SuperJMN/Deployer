using System.IO;
using CSharpFunctionalExtensions;

namespace Deployer.Wim
{
    public interface IWindowsImageMetadataReader
    {
        Result<XmlWindowsImageMetadata> Load(Stream stream);
    }
}