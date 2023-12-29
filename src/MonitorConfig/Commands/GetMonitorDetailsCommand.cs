using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "MonitorDetails")]
    [OutputType(typeof(MonitorDetails))]
    public sealed class GetMonitorDetailsCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public VCPMonitor[] Monitor { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var inputMonitor in Monitor)
            {
                var details = new MonitorDetails();
                try
                {
                    details.CapabilityInfo = ParsedCapabilityString.ParseCapabilityString(inputMonitor.GetCapabilitiesString());
                }
                catch (Win32Exception error)
                {
                    if (error.IsTerminatingError())
                    {
                        WriteError(new ErrorRecord(error, "FatalError", ErrorCategory.ResourceUnavailable, inputMonitor));
                        continue;
                    }

                    WriteWarning($"Failed to get capability info for monitor {inputMonitor.FriendlyName}");
                }

                try
                {
                    details.RefreshRate = inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.VerticalFrequency).CurrentValue / 100;
                }
                catch (Win32Exception)
                {
                    WriteWarning($"Failed to get refresh rate info for monitor {inputMonitor.FriendlyName}");
                }

                try
                {
                    details.DisplayUsageTime = new TimeSpan((int)inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.DisplayUsageTime).CurrentValue, 0, 0);
                }
                catch (Win32Exception)
                {
                    WriteWarning($"Failed to get powered on hours for monitor {inputMonitor.FriendlyName}");
                }

                try
                {
                    uint rawResponse = inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.DisplayControllerId).CurrentValue;
                    // First byte contains the Manufacturer ID, the rest is the actual controller ID.
                    // As a side note, there are 4 bytes in total in the response, in the documentation they are called: MH, ML, SH, SL.
                    details.ControllerManufacturer = MonitorDetails.GetDisplayControllerOEM((rawResponse << 24) >> 24);
                    details.ControllerID = rawResponse >> 8;
                }
                catch (Win32Exception)
                {
                    WriteWarning($"Failed to get display controller info for monitor {inputMonitor.FriendlyName}");
                }

                try
                {
                    uint rawResponse = inputMonitor.GetVCPFeatureResponse(KnownVcpCodes.DisplayFirmwareLevel).CurrentValue;
                    int version = (int)rawResponse >> 8;
                    int revision = (int)((rawResponse << 24) >> 24);
                    details.FirmwareVersion = new Version(version, revision);
                }
                catch (Win32Exception)
                {
                    WriteWarning($"Failed to get display controller firmware info for monitor {inputMonitor.FriendlyName}");
                }

                WriteObject(details);
            }
        }
    }
}