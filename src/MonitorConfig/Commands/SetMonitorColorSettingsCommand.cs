using MartinGC94.MonitorConfig.API.VCP;
using System.Management.Automation;
using MartinGC94.MonitorConfig.Native.Enums;
using MartinGC94.MonitorConfig.API;
using System;
using System.ComponentModel;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "MonitorColorSettings")]
    public sealed class SetMonitorColorSettingsCommand : PSCmdlet
    {
        #region parameters
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public VCPMonitor[] Monitor { get; set; }

        [Parameter()]
        public uint RedDrive { get; set; }

        [Parameter()]
        public uint RedGain { get; set; }

        [Parameter()]
        public uint GreenDrive { get; set; }

        [Parameter()]
        public uint GreenGain { get; set; }

        [Parameter()]
        public uint BlueDrive { get; set; }

        [Parameter()]
        public uint BlueGain { get; set; }

        [Parameter()]
        public uint RedSaturation { get; set; }

        [Parameter()]
        public uint YellowSaturation { get; set; }

        [Parameter()]
        public uint GreenSaturation { get; set; }

        [Parameter()]
        public uint CyanSaturation { get; set; }

        [Parameter()]
        public uint BlueSaturation { get; set; }

        [Parameter()]
        public uint MagentaSaturation { get; set; }

        [Parameter()]
        public uint ColorSaturation { get; set; }

        [Parameter()]
        public VCPMonitorColorTemperature ColorTemperature { get; set; }

        [Parameter()]
        public uint Contrast { get; set; }

        //[Parameter()]
        //public uint GrayScaleExpansion { get; set; }

        [Parameter()]
        public uint Gamma { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {
                foreach (var key in MyInvocation.BoundParameters.Keys)
                {
                    try
                    {
                        switch (key)
                        {
                            case "RedDrive":
                                inputMonitor.SetRGBDrive(Color.Red, RedDrive);
                                break;

                            case "RedGain":
                                inputMonitor.SetRGBGain(Color.Red, RedGain);
                                break;

                            case "GreenDrive":
                                inputMonitor.SetRGBDrive(Color.Green, GreenDrive);
                                break;

                            case "GreenGain":
                                inputMonitor.SetRGBGain(Color.Green, GreenGain);
                                break;

                            case "BlueDrive":
                                inputMonitor.SetRGBDrive(Color.Blue, BlueDrive);
                                break;

                            case "BlueGain":
                                inputMonitor.SetRGBGain(Color.Blue, BlueGain);
                                break;

                            case "RedSaturation":
                                inputMonitor.SetVCPValue(KnownVcpCodes.RedSaturation, RedSaturation);
                                break;

                            case "YellowSaturation":
                                inputMonitor.SetVCPValue(KnownVcpCodes.YellowSaturation, YellowSaturation);
                                break;

                            case "GreenSaturation":
                                inputMonitor.SetVCPValue(KnownVcpCodes.GreenSaturation, GreenSaturation);
                                break;

                            case "CyanSaturation":
                                inputMonitor.SetVCPValue(KnownVcpCodes.CyanSaturation, CyanSaturation);
                                break;

                            case "BlueSaturation":
                                inputMonitor.SetVCPValue(KnownVcpCodes.BlueSaturation, BlueSaturation);
                                break;

                            case "MagentaSaturation":
                                inputMonitor.SetVCPValue(KnownVcpCodes.MagentaSaturation, MagentaSaturation);
                                break;

                            case "ColorSaturation":
                                inputMonitor.SetVCPValue(KnownVcpCodes.ColorSaturation, ColorSaturation);
                                break;

                            case "ColorTemperature":
                                inputMonitor.SetMonitorColorTemperature(ColorTemperature);
                                break;

                            case "Contrast":
                                inputMonitor.SetContrastLevel(Contrast);
                                break;

                            //case "GrayScaleExpansion":
                            //    inputMonitor.SetVCPValue(KnownVcpCodes.GrayScaleExpansion, GrayScaleExpansion);
                            //    break;

                            case "Gamma":
                                inputMonitor.SetVCPValue(KnownVcpCodes.Gamma, Gamma);
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Win32Exception error)
                    {
                        WriteError(new ErrorRecord(
                        new Exception($"Failed to configure {key} due to error: {error.Message}", error),
                        "ColorConfigError",
                        Utils.GetErrorCategory(error),
                        inputMonitor));
                    }
                }
            }
        }
    }
}