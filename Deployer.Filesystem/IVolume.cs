using System.Collections.Generic;
using System.Threading.Tasks;
using ByteSizeLib;

namespace Zafiro.Storage
{
    public interface IVolume
    {
        string Root { get; }
        string Label { get; }
        FileSystemFormat FileSystemFormat { get; }
        ByteSize Size { get; }
        IPartition Partition { get; }
        Task Format(FileSystemFormat fileSystemFormat, string label);
        Task<ICollection<DriverMetadata>> GetDrivers();
    }
}