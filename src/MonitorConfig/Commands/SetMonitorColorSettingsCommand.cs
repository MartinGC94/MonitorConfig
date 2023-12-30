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

        [Parameter()]
        [ValidateRange(0, 3)]
        public uint GrayScaleWhiteExpansion
        {
            get
            {
                return _grayScaleWhiteExpansion;
            }
            set
            {
                _grayScaleWhiteExpansion = value << 8;
                _whiteGrayScaleSet = true;
            }
        }

        [Parameter()]
        [ValidateRange(0, 3)]
        public uint GrayScaleBlackExpansion
        {
            get
            {
                return _grayScaleBlackExpansion;
            }
            set
            {
                _grayScaleBlackExpansion = value;
                _blackGrayScaleSet = true;
            }
        }

        [Parameter()]
        public uint Gamma { get; set; }
        #endregion

        private uint _grayScaleWhiteExpansion;
        private uint _grayScaleBlackExpansion;
        private bool _whiteGrayScaleSet;
        private bool _blackGrayScaleSet;

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {
                if (_whiteGrayScaleSet || _blackGrayScaleSet)
                {
                    try
                    {
                        uint valueToSet;
                        if (_whiteGrayScaleSet && _blackGrayScaleSet)
                        {
                            valueToSet = _grayScaleBlackExpansion | _grayScaleWhiteExpansion;
                        }
                        else
                        {
                            valueToSet = inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.GrayScaleExpansion).CurrentValue;
                            // uint32 = 4 bytes: MH, ML, SH, SL
                            // Gray scale uses SL (Black region) and SH (White region)
                            // The logic here is to reset the bits in the region before resetting them with the specified value.
                            if (_blackGrayScaleSet)
                            {
                                valueToSet = ((valueToSet >> 8) << 8) | _grayScaleBlackExpansion;
                            }

                            if (_whiteGrayScaleSet)
                            {
                                uint tempValue = ((valueToSet << 24) >> 24) | _grayScaleWhiteExpansion;
                                valueToSet = (valueToSet >> 16 << 16) | tempValue;
                            }
                        }

                        inputMonitor.SetVCPValue(KnownVcpCodes.GrayScaleExpansion, valueToSet);
                    }
                    catch (Win32Exception error)
                    {
                        WriteError(new ErrorRecord(
                        new Exception($"Failed to configure GrayScaleExpansion due to error: {error.Message}", error),
                        "GrayScaleConfigError",
                        Utils.GetErrorCategory(error),
                        inputMonitor));
                    }
                }
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