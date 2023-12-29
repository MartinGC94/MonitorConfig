namespace MartinGC94.MonitorConfig.API.VCP
{
    public abstract class VCPFeatureInfo
    {
        public byte VCPCode { get; }
        public string Name { get; internal set; }
        public string Description { get; }
        public VCPCodeAccess Access { get; }

        internal VCPFeatureInfo(byte vcpCode)
        {
            VCPCode = vcpCode;

            if (VCPFeatureDocumentation.TryGetVCPFeatureDocumentation(vcpCode, out VCPFeatureDocumentation documentation))
            {
                Name = documentation.Name;
                Description = documentation.Description;
                Access = documentation.Access;
            }
            else
            {
                if (vcpCode > 0xDF)
                {
                    Name = "OEM specific";
                }
                else
                {
                    Name = "Reserved";
                }

                Description = string.Empty;
                Access = VCPCodeAccess.Unknown;
            }
        }

        public override string ToString()
        {
            return string.Format("{0:X2}", VCPCode) + " " + Name;
        }
    }
}
