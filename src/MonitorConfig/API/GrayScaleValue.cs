namespace MartinGC94.MonitorConfig.API
{
    public sealed class GrayScaleValue
    {
        internal uint rawNearWhiteValue;
        internal uint rawNearBlackValue;

        // Values 0-3 each represent one level of expansion. 4 and above are reserved must be ignored by the monitor.
        public uint NearWhiteLevel => rawNearWhiteValue > 3 ? 0 : rawNearWhiteValue;
        public uint NearBlackLevel => rawNearBlackValue > 3 ? 0 : rawNearBlackValue;

        internal GrayScaleValue(uint rawNearWhiteValue, uint rawNearBlackValue)
        {
            this.rawNearWhiteValue = rawNearWhiteValue;
            this.rawNearBlackValue = rawNearBlackValue;
        }

        public override string ToString()
        {
            return $"White: {NearWhiteLevel} Black: {NearBlackLevel}";
        }
    }
}