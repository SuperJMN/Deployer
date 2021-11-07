using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CSharpFunctionalExtensions;
using Serilog;
using Zafiro.Core.Pending;

namespace Deployer.Wim
{
    public abstract class WindowsImageMetadataReader : IWindowsImageMetadataReader
    {
        private static XmlSerializer Serializer { get; } = new XmlSerializer(typeof(WimMetadata));

        public Result<XmlWindowsImageMetadata> Load(Stream stream)
        {
            Log.Verbose("Getting WIM stream");

            WimMetadata metadata;
            try
            {
                metadata = (WimMetadata)Serializer.Deserialize(GetXmlMetadataStream(stream));
            }
            catch (Exception e)
            {
                return Result.Failure<XmlWindowsImageMetadata>("Could not read the metadata from the WIM " +
                                     $"file. Please, check it's a valid .WIM file: {e.Message}");
            }

            Log.Verbose("Wim metadata deserialized correctly {@Metadata}", metadata);

            var images = from i in metadata.Images.Where(t => t.Windows is not null)
                from a in GetArchitecture(i.Windows.Arch).ToEnumerable()
                select new DiskImageMetadata
                {
                    Architecture = a,
                    Build = i.Windows.Version.Build,
                    DisplayName = i.Name,
                    Index = int.Parse(i.Index)
                };

            return new XmlWindowsImageMetadata
            {
                Images = images.ToList(),
            };
        }

        private static Result<MyArchitecture> GetArchitecture(string str)
        {
            switch (str)
            {
                case "0":
                    return MyArchitecture.X86;
                case "9":
                    return MyArchitecture.X64;
                case "12":
                    return MyArchitecture.Arm64;
            }

            return Result.Failure<MyArchitecture>($"Cannot find architecture '{str}' is unknown");
        }

        protected abstract Stream GetXmlMetadataStream(Stream wim);
    }
}