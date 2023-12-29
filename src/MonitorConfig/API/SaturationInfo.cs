using MartinGC94.MonitorConfig.API.VCP;

namespace MartinGC94.MonitorConfig.API
{
    public sealed class SaturationInfo
    {
        public VCPValue ColorSaturation { get; internal set; }
        public VCPValue RedSaturation { get; internal set; }
        public VCPValue YellowSaturation { get; internal set; }
        public VCPValue GreenSaturation { get; internal set; }
        public VCPValue CyanSaturation { get; internal set; }
        public VCPValue BlueSaturation { get; internal set; }
        public VCPValue MagentaSaturation { get; internal set; }

        internal SaturationInfo() {}
    }
}