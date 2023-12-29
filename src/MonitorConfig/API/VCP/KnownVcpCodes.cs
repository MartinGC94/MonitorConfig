namespace MartinGC94.MonitorConfig.API.VCP
{
    /// <summary>
    /// VCP codes used by internal commands. Full list of codes is in VCPFeatures.csv
    /// </summary>
    internal sealed class KnownVcpCodes
    {
        public const byte ColorSaturation = 0x8A;
        public const byte RedSaturation = 0x59;
        public const byte YellowSaturation = 0x5A;
        public const byte GreenSaturation = 0x5B;
        public const byte CyanSaturation = 0x5C;
        public const byte BlueSaturation = 0x5D;
        public const byte MagentaSaturation = 0x5E;
        public const byte GrayScaleExpansion = 0x2E;
        public const byte Gamma = 0x72;
        public const byte RestoreFactoryLuminanceAndContrastDefaults = 0x05;
        public const byte VerticalFrequency = 0xAE;
        public const byte DisplayUsageTime = 0xC0;
        public const byte DisplayControllerId = 0xC8;
        public const byte DisplayFirmwareLevel = 0xC9;
    }
}