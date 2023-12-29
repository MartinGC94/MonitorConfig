namespace MartinGC94.MonitorConfig.API
{
    public sealed class ColorData
    {
        public ColorValue Red { get; internal set; }
        public ColorValue Green { get; internal set; }
        public ColorValue Blue { get; internal set; }

        internal ColorData() { }
    }
}