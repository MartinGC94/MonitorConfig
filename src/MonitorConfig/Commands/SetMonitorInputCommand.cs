using MartinGC94.MonitorConfig.API.ParamAttributes;
using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "MonitorInput")]
    public sealed class SetMonitorInputCommand : PSCmdlet
    {
        #region parameters
        [MonitorArgTransformer()]
        [ArgumentCompleter(typeof(VCPDeviceNameCompleter))]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public VCPMonitor[] Monitor { get; set; }

        [MonitorInputSourceArgTransformer()]
        [ArgumentCompleter(typeof(MonitorInputSourceCompleter))]
        [Parameter(Mandatory = true, Position = 1)]
        public uint Source { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {
                try
                {
                    inputMonitor.SetInputSource(Source);
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(
                        new Exception($"Failed to set input source due to error: {error.Message}", error),
                        "SetInputError",
                        Utils.GetErrorCategory(error),
                        inputMonitor));
                }
            }
        }
    }
}
