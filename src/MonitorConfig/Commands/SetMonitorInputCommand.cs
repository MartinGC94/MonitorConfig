using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "MonitorInput", DefaultParameterSetName = "BySource")]
    public sealed class SetMonitorInputCommand : PSCmdlet
    {
        #region parameters
        [MonitorArgTransformer()]
        [ArgumentCompleter(typeof(DeviceNameCompleter))]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public VCPMonitor[] Monitor { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "BySource", Position = 1)]
        public MonitorInputSource Source { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ByValue", Position = 1)]
        public uint Value { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            uint targetValue = ParameterSetName.Equals("BySource") ? (uint)Source : Value;

            foreach (VCPMonitor inputMonitor in Monitor)
            {
                try
                {
                    inputMonitor.SetInputSource(targetValue);
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
