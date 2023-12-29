using MartinGC94.MonitorConfig.Native.Enums;

namespace MartinGC94.MonitorConfig.API.VCP
{
    public sealed class CapabilityInfo
    {
        public bool SupportsBrightnessControl { get; }
        public bool SupportsColorTemperatureControl { get; }
        public bool SupportsContrastControl { get; }
        public bool SupportsDegaussFunction { get; }
        public bool SupportsDisplayAreaPositionAdjustments { get; }
        public bool SupportsDisplayAreaSizeAdjustments { get; }
        public bool CanReportPanelType { get; }
        public bool SupportsRGBDriveAdjustments { get; }
        public bool SupportsRGBGainAdjustments { get; }
        public bool CanResetColorsToDefault { get; }
        public bool CanResetAllSettingsToDefault { get; }
        public bool FactoryDefaultsEnablesDisabledFeatures { get; }
        public VCPMonitorColorTemperature SupportedColorTemperatures { get; }

        internal CapabilityInfo(VCPMonitorCapabilities monitorCapabilities, VCPMonitorColorTemperature colorTemps)
        {
            SupportsBrightnessControl = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_BRIGHTNESS) != 0;
            SupportsColorTemperatureControl = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_COLOR_TEMPERATURE) != 0;
            SupportsContrastControl = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_CONTRAST) != 0;
            SupportsDegaussFunction = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_DEGAUSS) != 0;
            SupportsDisplayAreaPositionAdjustments = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_DISPLAY_AREA_POSITION) != 0;
            SupportsDisplayAreaSizeAdjustments = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_DISPLAY_AREA_SIZE) != 0;
            CanReportPanelType = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_MONITOR_TECHNOLOGY_TYPE) != 0;
            SupportsRGBDriveAdjustments = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_RED_GREEN_BLUE_DRIVE) != 0;
            SupportsRGBGainAdjustments = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_RED_GREEN_BLUE_GAIN) != 0;
            CanResetColorsToDefault = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_RESTORE_FACTORY_COLOR_DEFAULTS) != 0;
            CanResetAllSettingsToDefault = (monitorCapabilities & VCPMonitorCapabilities.MC_CAPS_RESTORE_FACTORY_DEFAULTS) != 0;
            FactoryDefaultsEnablesDisabledFeatures = (monitorCapabilities & VCPMonitorCapabilities.MC_RESTORE_FACTORY_DEFAULTS_ENABLES_MONITOR_SETTINGS) != 0;
            SupportedColorTemperatures = colorTemps;
        }
    }
}
