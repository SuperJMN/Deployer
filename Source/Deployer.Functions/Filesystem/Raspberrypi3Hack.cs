using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Zafiro.Storage;

namespace Deployer.Functions.Filesystem;

public static class Raspberrypi3Hack
{
    private const long EspFirstBlock = 0x100000L;
    private const int SectorSizeInBytes = 512;

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        [MarshalAs(UnmanagedType.LPTStr)] string filename,
        [MarshalAs(UnmanagedType.U4)] FileAccess access,
        [MarshalAs(UnmanagedType.U4)] FileShare share,
        IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
        IntPtr templateFile);


    public static void Apply(IDisk disk)
    {
        using var fileStream = GetDiskStream(disk);
        var bpb = GetBpb(fileStream);
        var newBpb = CreateNewBpb(bpb);
        WriteToProtectiveMbr(newBpb, fileStream);
    }

    private static FileStream GetDiskStream(IDisk disk)
    {
        var diskHandle = CreateFile("\\\\.\\PhysicalDrive" + disk.Number, FileAccess.ReadWrite, FileShare.ReadWrite,
            IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);

        if (diskHandle.IsInvalid)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        return new FileStream(diskHandle, FileAccess.ReadWrite);
    }

    private static byte[] GetBpb(Stream fileStream)
    {
        return BoundaryBasedRead(EspFirstBlock, 11, 79, fileStream);
    }

    private static byte[] CreateNewBpb(byte[] bpb)
    {
        var newReservedLogicSectors = GetReservedSectors(bpb);

        return Concat(
            bpb[..3],
            BitConverter.GetBytes(newReservedLogicSectors),
            bpb[5..]);
    }

    private static short GetReservedSectors(byte[] bpb)
    {
        var copy = (byte[]) bpb.Clone();
        var reservedLogicSectorsArray = copy[3..5];
        var reservedLogicSectors = BitConverter.ToInt16(reservedLogicSectorsArray);
        reservedLogicSectors += 2048;

        return reservedLogicSectors;
    }

    private static void WriteToProtectiveMbr(byte[] bpb, Stream fileStream)
    {
        BoundaryBasedWrite(0, 11, bpb, fileStream);
    }

    private static void BoundaryBasedWrite(long boundary, int desiredOffset, byte[] data, Stream stream)
    {
        stream.Seek(boundary, SeekOrigin.Begin);
        var existing = new byte[SectorSizeInBytes];
        stream.Read(existing, 0, existing.Length);

        var toWrite = Concat(
            existing[..desiredOffset], 
            data, 
            existing[(desiredOffset+data.Length)..]);
        stream.Seek(boundary, SeekOrigin.Begin);
        stream.Write(toWrite, 0, toWrite.Length);
    }

    private static byte[] BoundaryBasedRead(long boundary, int desiredOffset, int count, Stream stream)
    {
        stream.Seek(boundary, SeekOrigin.Begin);
        var contents = new byte[desiredOffset + count];
        stream.Read(contents, 0, contents.Length);

        return contents[desiredOffset..];
    }

    private static byte[] Concat(params byte[][] segments)
    {
        return segments
            .SelectMany(b => b)
            .ToArray();
    }
}