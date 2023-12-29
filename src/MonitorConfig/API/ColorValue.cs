namespace MartinGC94.MonitorConfig.API
{
    public sealed class ColorValue
    {
        public uint CurrentValue { get; }
        public uint MinimumValue { get; }
        public uint MaximumValue { get; }

        internal ColorValue(uint current, uint min, uint max)
        {
            CurrentValue = current;
            MinimumValue = min;
            MaximumValue = max;
        }

        public override string ToString()
        {
            return $"Current {CurrentValue} Min {MinimumValue} Max {MaximumValue}";
        }
    }
}