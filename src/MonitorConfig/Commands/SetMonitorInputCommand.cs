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
        [ArgumentCompleter(typeof(MonitorInputSourceCompleter))]
        public string Source { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ByValue", Position = 1)]
        [ArgumentCompleter(typeof(MonitorInputValueCompleter))]
        public uint Value { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            uint targetValue;
            if (ParameterSetName.Equals("BySource"))
            {
                if (!Enum.TryParse<MonitorInputSource>(Source, true, out MonitorInputSource parsed))
                {
                    string validValues = string.Join(", ", Enum.GetNames(typeof(MonitorInputSource)));
                    ThrowTerminatingError(new ErrorRecord(
                        new ArgumentException(
                            $"'{Source}' is not a valid MonitorInputSource. Valid values: {validValues}. " +
                            "For values outside the MCCS spec (e.g. USB-C), use the -Value parameter instead."),
                        "InvalidInputSource",
                        ErrorCategory.InvalidArgument,
                        Source));
                    return;
                }
                targetValue = (byte)parsed;
            }
            else
            {
                targetValue = Value;
            }

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
