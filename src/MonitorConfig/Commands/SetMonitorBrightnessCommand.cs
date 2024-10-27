using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using MartinGC94.MonitorConfig.API.WMI;
using System.Management.Automation;
using System;
using System.ComponentModel;
using Microsoft.Management.Infrastructure;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "MonitorBrightness", DefaultParameterSetName = "Default")]
    public sealed class SetMonitorBrightnessCommand : PSCmdlet
    {
        #region parameters
        [MonitorArgTransformer()]
        [ArgumentCompleter(typeof(DeviceNameCompleter))]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public Monitor[] Monitor { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Default", Position = 1)]
        [Parameter(Mandatory = false, ParameterSetName = "WMIOptions", Position = 1)]
        public uint Value { get; set; }

        [Parameter(ParameterSetName = "WMIOptions")]
        public uint Timeout { get; set; } = 1;

        [Parameter(ParameterSetName = "WMIOptions")]
        public SwitchParameter RevertToPolicy { get; set; }

        [Parameter(ParameterSetName = "WMIOptions")]
        public bool ALSEnabled { get; set; }

        [Parameter(ParameterSetName = "WMIOptions")]
        public byte ALSBrightness { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (Monitor inputMonitor in Monitor)
            {
                if (inputMonitor is WMIMonitor wmiMonitor)
                {
                    if (RevertToPolicy)
                    {
                        try
                        {
                            wmiMonitor.RevertToPolicyBrightness();
                        }
                        catch (CimException error)
                        {
                            WriteError(new ErrorRecord(
                            new Exception($"Failed to revert policy due to error: {error.Message}", error),
                            "RevertPolicyError",
                            Utils.GetErrorCategory(error),
                            wmiMonitor));
                        }
                    }
                    
                    if (MyInvocation.BoundParameters.ContainsKey("ALSEnabled"))
                    {
                        try
                        {
                            wmiMonitor.SetAmbientLightSensorState(ALSEnabled);
                        }
                        catch (CimException error)
                        {
                            WriteError(new ErrorRecord(
                            new Exception($"Failed to modify ALS state due to error: {error.Message}", error),
                            "SetALSStateError",
                            Utils.GetErrorCategory(error),
                            wmiMonitor));
                        }
                    }

                    if (MyInvocation.BoundParameters.ContainsKey("ALSBrightness"))
                    {
                        try
                        {
                            wmiMonitor.SetAmbientLightSensorBrightnessLevel(ALSBrightness);
                        }
                        catch (CimException error)
                        {
                            WriteError(new ErrorRecord(
                            new Exception($"Failed to modify ALS level due to error: {error.Message}", error),
                            "SetALSLevelError",
                            Utils.GetErrorCategory(error),
                            wmiMonitor));
                        }
                    }

                    if (MyInvocation.BoundParameters.ContainsKey("Value"))
                    {
                        try
                        {
                            wmiMonitor.SetBrightnessLevel((byte)Value, Timeout);
                        }
                        catch (CimException error)
                        {
                            WriteError(new ErrorRecord(
                            new Exception($"Failed to change brightness level due to error: {error.Message}", error),
                            "SetWmiBrightnessError",
                            Utils.GetErrorCategory(error),
                            wmiMonitor));
                        }
                    }
                }
                else if (inputMonitor is VCPMonitor vcpMonitor)
                {
                    if (ParameterSetName.Equals("WMIOptions"))
                    {
                        WriteError(new ErrorRecord(
                            new Exception($"WMI options cannot be set on display \"{vcpMonitor.InstanceName}\" because it is controlled through VCP"),
                            "WMIOptionsSetOnVCPDisplay",
                            ErrorCategory.InvalidOperation,
                            vcpMonitor));
                        continue;
                    }

                    try
                    {
                        vcpMonitor.SetBrightnessLevel(Value);
                    }
                    catch (Win32Exception error)
                    {
                        WriteError(new ErrorRecord(
                        new Exception($"Failed to change brightness level due to error: {error.Message}", error),
                        "SetBrightnessError",
                        Utils.GetErrorCategory(error),
                        vcpMonitor));
                    }
                }
            }
        }
    }
}