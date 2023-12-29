using System;
using System.Runtime.InteropServices;

namespace MartinGC94.MonitorConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct PhysicalMonitor
    {
        public IntPtr hPhysicalMonitor;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szPhysicalMonitorDescription;

        public override string ToString()
        {
            return szPhysicalMonitorDescription;
        }
    }
}