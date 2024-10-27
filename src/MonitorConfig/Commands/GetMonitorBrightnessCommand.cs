using MartinGC94.MonitorConfig.API;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "MonitorBrightness")]
    [OutputType(typeof(BrightnessInfo))]
    public sealed class GetMonitorBrightnessCommand : Cmdlet
    {
        #region parameters
        [MonitorArgTransformer()]
        [ArgumentCompleter(typeof(DeviceNameCompleter))]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public Monitor[] Monitor { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (var inputMonitor in Monitor)
            {
                try
                {
                    WriteObject(inputMonitor.GetBrightnessInfo());
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(
                    new Exception($"Failed to get brightness info due to error: {error.Message}", error),
                    "GetBrightnessError",
                    Utils.GetErrorCategory(error),
                    inputMonitor));
                }
            }
        }
    }
}