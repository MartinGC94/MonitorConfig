using MartinGC94.MonitorConfig.Native.Enums;

namespace MartinGC94.MonitorConfig.API.VCP
{
    public sealed class VCPFeatureResponse : VCPFeatureInfo
    {
        public VCPCodeType CodeType { get; }
        public uint CurrentValue { get; }
        public uint MaxValue { get; }

        internal VCPFeatureResponse(byte vcpCode, VCPCodeType type, uint currentValue, uint maxValue) : base(vcpCode)
        {
            CodeType = type;
            CurrentValue = currentValue;
            MaxValue = maxValue;
        }
    }
}
