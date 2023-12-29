using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using MartinGC94.MonitorConfig.Native.Enums;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "MonitorColorSettings")]
    [OutputType(typeof(MonitorColorInfo))]
    public sealed class GetMonitorColorSettingsCommand : Cmdlet
    {
        #region parameters
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public VCPMonitor[] Monitor { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {
                var driveInfo = new ColorData();
                try
                {
                    driveInfo.Red = inputMonitor.GetRGBDriveInfo(Color.Red);
                    driveInfo.Green = inputMonitor.GetRGBDriveInfo(Color.Green);
                    driveInfo.Blue = inputMonitor.GetRGBDriveInfo(Color.Blue);
                }
                catch (Win32Exception error)
                {
                    if (error.IsTerminatingError())
                    {
                        WriteError(new ErrorRecord(error, "FatalError", ErrorCategory.ResourceUnavailable, inputMonitor));
                        continue;
                    }

                    driveInfo = null;
                    WriteWarning($"Failed to get drive info for monitor {inputMonitor.FriendlyName}");
                }

                var gainInfo = new ColorData();
                try
                {
                    gainInfo.Red = inputMonitor.GetRGBGainInfo(Color.Red);
                    gainInfo.Green = inputMonitor.GetRGBGainInfo(Color.Green);
                    gainInfo.Blue = inputMonitor.GetRGBGainInfo(Color.Blue);
                }
                catch (Win32Exception)
                {
                    gainInfo = null;
                    WriteWarning($"Failed to get gain info for monitor {inputMonitor.FriendlyName}");
                }

                var saturationInfo = new SaturationInfo();
                try
                {
                    saturationInfo.ColorSaturation = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.ColorSaturation));
                }
                catch (Win32Exception)
                {
                    WriteWarning($"Failed to get general saturation info for monitor {inputMonitor.FriendlyName}");
                }

                try
                {
                    saturationInfo.RedSaturation = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.RedSaturation));
                    saturationInfo.YellowSaturation = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.YellowSaturation));
                    saturationInfo.GreenSaturation = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.GreenSaturation));
                    saturationInfo.CyanSaturation = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.CyanSaturation));
                    saturationInfo.BlueSaturation = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.BlueSaturation));
                    saturationInfo.MagentaSaturation = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.MagentaSaturation));
                }
                catch (Win32Exception)
                {
                    WriteWarning($"Failed to get color specific saturation info for monitor {inputMonitor.FriendlyName}");
                }

                VCPMonitorColorTemperature colorTemp;
                try
                {
                    colorTemp = inputMonitor.GetMonitorColorTemperature();
                }
                catch (Win32Exception)
                {
                    colorTemp = VCPMonitorColorTemperature._None;
                    WriteWarning($"Failed to get color temperature info for monitor {inputMonitor.FriendlyName}");
                }

                ContrastInfo contrastInfo;
                try
                {
                    contrastInfo = inputMonitor.GetContrastInfo();
                }
                catch (Win32Exception)
                {
                    contrastInfo = null;
                    WriteWarning($"Failed to get contrast info for monitor {inputMonitor.FriendlyName}");
                }

                //VCPValue grayScale;
                //try
                //{
                //    grayScale = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.GrayScaleExpansion));
                //}
                //catch (Win32Exception)
                //{
                //    grayScale = null;
                //    WriteWarning($"Failed to get gray scale expansion response from monitor {inputMonitor.FriendlyName}");
                //}

                VCPValue gamma;
                try
                {
                    gamma = new VCPValue(inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.Gamma));
                }
                catch (Win32Exception)
                {
                    gamma = null;
                    WriteWarning($"Failed to get gamma response from monitor {inputMonitor.FriendlyName}");
                }


                WriteObject(new MonitorColorInfo(driveInfo, gainInfo, saturationInfo, colorTemp, contrastInfo/*, grayScale*/, gamma));
            }
        }
    }
}