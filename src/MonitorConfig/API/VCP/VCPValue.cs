namespace MartinGC94.MonitorConfig.API.VCP
{
    public sealed class VCPValue
    {
        public uint CurrentValue { get; }
        public uint MaxValue { get; }

        internal VCPValue(VCPFeatureResponse vcpResponse)
        {
            CurrentValue = vcpResponse.CurrentValue;
            MaxValue = vcpResponse.MaxValue;
        }

        public override string ToString()
        {
            return $"Current: {CurrentValue} Max: {MaxValue}";
        }
    }
}