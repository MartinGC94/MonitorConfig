using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "MonitorVCPResponse")]
    [OutputType(typeof(VCPFeatureResponse))]
    public sealed class GetMonitorVCPResponseCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public VCPMonitor[] Monitor { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "SpecifiedCode")]
        [ArgumentCompleter(typeof(VCPCodeCompleter))]
        public byte[] VCPCode { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "All")]
        public SwitchParameter All { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var inputMonitor in Monitor)
            {
                if (All)
                {
                    int codesFound = 0;
                    string activity = $"Scanning codes on {inputMonitor.FriendlyName}";
                    try
                    {
                        foreach (VCPFeatureResponse response in inputMonitor.ScanVCPFeatures())
                        {
                            codesFound++;
                            var progress = new ProgressRecord(0, activity, $"{codesFound} codes found")
                            {
                                PercentComplete = (int)Math.Round((double)response.VCPCode / byte.MaxValue * 100)
                            };
                            WriteProgress(progress);
                            WriteObject(response);
                        }
                    }
                    catch (Win32Exception error)
                    {
                        WriteError(new ErrorRecord(
                        new Exception($"Failed to enumerate the VCP features due to error: {error.Message}", error),
                        "EnumerateVCPError",
                        Utils.GetErrorCategory(error),
                        inputMonitor));
                    }
                    finally
                    {
                        var progress = new ProgressRecord(0, activity, "Done")
                        {
                            RecordType = ProgressRecordType.Completed
                        };
                        WriteProgress(progress);
                    }

                    continue;
                }

                foreach (var code in VCPCode)
                {
                    try
                    {
                        WriteObject(inputMonitor.GetVCPFeatureResponse(code));
                    }
                    catch (Win32Exception error)
                    {
                        WriteError(new ErrorRecord(
                        new Exception($"Failed to get data for code {code} due to error: {error.Message}", error),
                        "VCPResponseError",
                        Utils.GetErrorCategory(error),
                        inputMonitor));
                    }
                }
            }
        }
    }
}