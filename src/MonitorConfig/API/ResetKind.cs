namespace MartinGC94.MonitorConfig.API
{
    public enum ResetKind : byte
    {
        All = 0x04,
        BrightnessAndContrast = 0x05,
        Geometry = 0x06,
        Colors = 0x08,
        TVDefaults = 0x0A
    }
}