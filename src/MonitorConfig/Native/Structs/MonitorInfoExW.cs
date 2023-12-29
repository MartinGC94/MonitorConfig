using System.Runtime.InteropServices;

namespace MartinGC94.MonitorConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MonitorInfoExW
    {
        public uint cbSize;

        public RECTL rcMonitor;

        public RECTL rcWork;

        public uint dwFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }
}