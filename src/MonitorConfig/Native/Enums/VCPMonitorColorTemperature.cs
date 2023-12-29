using System;

namespace MartinGC94.MonitorConfig.Native.Enums
{
    [Flags()]
    public enum VCPMonitorColorTemperature : uint
    {
        _None   = 0x00000000,
        _4000k  = 0x00000001,
        _5000k  = 0x00000002,
        _6500k  = 0x00000004,
        _7500k  = 0x00000008,
        _8200k  = 0x00000010,
        _9300k  = 0x00000020,
        _10000k = 0x00000040,
        _11500k = 0x00000080
    }
}