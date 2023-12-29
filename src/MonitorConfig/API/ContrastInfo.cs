namespace MartinGC94.MonitorConfig.API
{
    public sealed class ContrastInfo
    {
        public uint CurrentContrast { get; }
        public uint MinimumContrast { get; }
        public uint MaximumContrast { get; }

        internal ContrastInfo(uint currentContrast, uint minContrast, uint maxContrast)
        {
            CurrentContrast = currentContrast;
            MinimumContrast = minContrast;
            MaximumContrast = maxContrast;
        }

        public override string ToString()
        {
            return $"Current {CurrentContrast} Min {MinimumContrast} Max {MaximumContrast}";
        }
    }
}
