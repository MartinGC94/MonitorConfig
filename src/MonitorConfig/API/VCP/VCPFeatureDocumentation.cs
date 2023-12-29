namespace MartinGC94.MonitorConfig.API.VCP
{
    internal sealed partial class VCPFeatureDocumentation
    {
        public string Name { get; }
        public VCPCodeAccess Access { get; }
        public string Description { get; }

        private VCPFeatureDocumentation(string name, VCPCodeAccess access, string description)
        {
            Name = name;
            Access = access;
            Description = description;
        }

        internal static bool TryGetVCPFeatureDocumentation(byte vcpCode, out VCPFeatureDocumentation documentation)
        {
            // The documentationTable is generated via the VCPFeatureDocumentationGenerator project using the contents of the VCPFeatures.csv file
            if (documentationTable.TryGetValue(vcpCode, out VCPFeatureDocumentation data))
            {
                documentation = data;
                return true;
            }

            documentation = null;
            return false;
        }
    }
}