using MartinGC94.MonitorConfig.API.ParamAttributes;
using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "MonitorInput")]
    [OutputType(typeof(MonitorInputInfo))]
    public sealed class GetMonitorInputCommand : Cmdlet
    {
        #region parameters
        [MonitorArgTransformer()]
        [ArgumentCompleter(typeof(VCPDeviceNameCompleter))]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public VCPMonitor[] Monitor { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {
                byte[] possibleValues = MonitorInputCompleterHelper.TryGetSupportedValues(inputMonitor);

                try
                {
                    WriteObject(inputMonitor.GetInputInfo(possibleValues));
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(
                        new Exception($"Failed to get input source due to error: {error.Message}", error),
                        "GetInputError",
                        Utils.GetErrorCategory(error),
                        inputMonitor));
                }
            }
        }
    }
}
