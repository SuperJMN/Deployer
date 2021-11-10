using System;
using System.IO;
using Zafiro.Storage.Windows.Gpt;

namespace Zafiro.Storage.Windows
{
    public class Utils
    {
        public static void ConfigureAsSuperFloppy(IDisk disk)
        {
            using (var deviceStream = new DeviceStream("\\\\.\\PhysicalDrive" + disk.Number, FileAccess.ReadWrite))
            {
                var buffer1 = new byte[87];
                deviceStream.Seek(0x100000, SeekOrigin.Begin);
                deviceStream.Read(buffer1, 0, buffer1.Length);
                Buffer.BlockCopy(BitConverter.GetBytes((short)(BitConverter.ToInt16(new[]
                {
                    buffer1[14],
                    buffer1[15]
                }, 0) + 2048)), 0, buffer1, 14, 2);
                var buffer2 = new byte[512];
                deviceStream.Seek(0L, SeekOrigin.Begin);
                deviceStream.Read(buffer2, 0, buffer2.Length);
                Buffer.BlockCopy(buffer1, 11, buffer2, 11, 76);
                deviceStream.Seek(0L, SeekOrigin.Begin);
                deviceStream.Write(buffer2, 0, buffer2.Length);
            }
        }
    }
}