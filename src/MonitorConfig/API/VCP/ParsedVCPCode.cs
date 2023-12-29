namespace MartinGC94.MonitorConfig.API.VCP
{
    public sealed class ParsedVCPCode : VCPFeatureInfo
    {
        public byte[] ValidValues { get; internal set; }

        internal ParsedVCPCode(byte vcpCode) : base(vcpCode) { }
    }
}