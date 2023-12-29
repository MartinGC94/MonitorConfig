namespace MartinGC94.MonitorConfig.API
{
    public sealed class BrightnessInfo
    {
        public uint CurrentBrightness { get; }
        public uint MinimumBrightness { get; }
        public uint MaximumBrightness { get; }

        internal BrightnessInfo(uint currentBrightness, uint minBrightness, uint maxBrightness)
        {
            CurrentBrightness = currentBrightness;
            MinimumBrightness = minBrightness;
            MaximumBrightness = maxBrightness;
        }

        public override string ToString()
        {
            return $"Current {CurrentBrightness} Min {MinimumBrightness} Max {MaximumBrightness}";
        }
    }
}
