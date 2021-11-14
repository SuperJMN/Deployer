﻿using System;
using System.Runtime.InteropServices;
using static System.Diagnostics.Process;

namespace Zafiro.System.Windows
{
    public static class ArchitectureInfo
    {
        public static bool IsArm64()
        {
            var handle = GetCurrentProcess().Handle;
            IsWow64Process2(handle, out var processMachine, out var nativeMachine);

            return nativeMachine == 0xaa64;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool IsWow64Process2(
            IntPtr process,
            out ushort processMachine,
            out ushort nativeMachine
        );
    }
}
