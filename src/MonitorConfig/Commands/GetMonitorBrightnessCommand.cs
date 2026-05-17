using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.ParamAttributes;
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
                BrightnessInfo info;
                try
                {
                    info = inputMonitor.GetBrightnessInfo();
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(
                    new ApiException($"Failed to get brightness info due to error: {error.Message}", error),
                    "GetBrightnessError",
                    error.GetErrorCategory(),
                    inputMonitor));
                    continue;
                }

                WriteObject(info);
            }
        }
    }
}