using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using CSharpFunctionalExtensions;
using ManagedWimLib;
using Serilog;
using Zafiro.Core.Pending;
using Zafiro.System.Windows;

namespace Deployer.Wim;

public class WindowsImageMetadataReader : IWindowsImageMetadataReader
{
    private static bool isInitialized;

    public WindowsImageMetadataReader()
    {
        if (isInitialized)
        {
            return;
        }

        InitNativeLibrary();
        isInitialized = true;
    }

    private static XmlSerializer Serializer { get; } = new(typeof(WimMetadata));

    public Result<XmlWindowsImageMetadata> Load(string path)
    {
        return Result.Try(() => LoadCore(path));
    }

    private static void InitNativeLibrary()
    {
        var libBaseDir = AppDomain.CurrentDomain.BaseDirectory;
        var libDir = "runtimes";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            libDir = Path.Combine(libDir, "win-");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            libDir = Path.Combine(libDir, "linux-");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            libDir = Path.Combine(libDir, "osx-");
        }

        switch (RuntimeInformation.ProcessArchitecture)
        {
            case Architecture.X86:
                libDir += "x86";
                break;
            case Architecture.X64:
                libDir += "x64";
                break;
            case Architecture.Arm:
                libDir += "arm";
                break;
            case Architecture.Arm64:
                libDir += "arm64";
                break;
        }

        libDir = Path.Combine(libDir, "native");

        // Some platforms require native library custom path to be an absolute path.
        string libPath = null;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            libPath = Path.Combine(libBaseDir, libDir, "libwim-15.dll");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            libPath = Path.Combine(libBaseDir, libDir, "libwim.so");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            libPath = Path.Combine(libBaseDir, libDir, "libwim.dylib");
        }

        if (libPath == null)
        {
            throw new PlatformNotSupportedException("Unable to find native library.");
        }

        if (!File.Exists(libPath))
        {
            throw new PlatformNotSupportedException($"Unable to find native library [{libPath}].");
        }

        ManagedWimLib.Wim.GlobalInit(libPath, InitFlags.None);
    }

    private static Result<ProcessorArchitecture> GetArchitecture(string str)
    {
        switch (str)
        {
            case "0":
                return ProcessorArchitecture.X86;
            case "9":
                return ProcessorArchitecture.Amd64;
            case "12":
                return ProcessorArchitecture.Arm64;
        }

        return Result.Failure<ProcessorArchitecture>($"Cannot find architecture '{str}' is unknown");
    }

    private XmlWindowsImageMetadata LoadCore(string path)
    {
        var xml = ManagedWimLib.Wim.OpenWim(path, OpenFlags.None).GetXmlData()[1..]; // Skip first (invalid) invisible char
        var stringReader = new StringReader(xml);

        var metadata = (WimMetadata) Serializer.Deserialize(stringReader);

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
            Images = images.ToList()
        };
    }
}