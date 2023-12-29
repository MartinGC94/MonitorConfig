using System.Runtime.InteropServices;

namespace MartinGC94.MonitorConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DisplayDevice
    {
        public uint cb;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string deviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string deviceString;

        public uint stateFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string deviceID;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string deviceKey;
    }
}