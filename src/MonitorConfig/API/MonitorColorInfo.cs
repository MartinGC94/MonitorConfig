using MartinGC94.MonitorConfig.API.VCP;
using MartinGC94.MonitorConfig.Native.Enums;

namespace MartinGC94.MonitorConfig.API
{
    public sealed class MonitorColorInfo
    {
        public ColorData DriveInfo { get; }
        public ColorData GainInfo { get; }
        public SaturationInfo SaturationInfo { get; }
        public VCPMonitorColorTemperature ColorTemperature { get; }
        public ContrastInfo ContrastInfo { get; }
        //public VCPValue GrayScaleExpansion { get; }
        public VCPValue Gamma { get; }

        internal MonitorColorInfo(
            ColorData driveData,
            ColorData gainData,
            SaturationInfo saturationData,
            VCPMonitorColorTemperature colorTemp,
            ContrastInfo contrastData,
            //VCPValue grayScaleData,
            VCPValue gammaData)
        {
            DriveInfo = driveData;
            GainInfo = gainData;
            SaturationInfo = saturationData;
            ColorTemperature = colorTemp;
            ContrastInfo = contrastData;
            //GrayScaleExpansion = grayScaleData;
            Gamma = gammaData;
        }
    }
}