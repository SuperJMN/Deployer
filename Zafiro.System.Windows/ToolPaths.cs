﻿using System;
using System.IO;

namespace Zafiro.System.Windows
{
    public static class ToolPaths
    {
        public static string BcdEdit { get; } = Path.Combine(GetSystemFolder, "bcdedit.exe");
        public static string BcdBoot { get; } = Path.Combine(GetSystemFolder, "bcdboot.exe");
        public static string Dism { get; } = GetDismPath();

        private static string GetDismPath()
        {
            var bundledDism = Path.Combine("Dism", OsMetadata.Architecture.ToString(), "DISM.exe");
            if (File.Exists(bundledDism))
            {
                return bundledDism;
            }

            return Path.Combine(GetSystemFolder, "dism.exe");
        }

        private static string GetSystemFolder
        {
            get
            {
                var sysNativeFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysNative");

                if (Is64OsRunning32Proc())
                {
                    return sysNativeFolder;
                }

                if (ArchitectureInfo.IsArm64())
                {
                    return sysNativeFolder;
                }

                return Path.Combine(Environment.SystemDirectory);
            }
        }

        private static bool Is64OsRunning32Proc()
        {
            return Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess;
        }
    }
}