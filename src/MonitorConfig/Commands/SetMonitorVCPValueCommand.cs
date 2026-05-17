using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.ParamAttributes;
using MartinGC94.MonitorConfig.API.VCP;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "MonitorVCPValue")]
    public sealed class SetMonitorVCPValueCommand : Cmdlet
    {
        #region parameters
        [MonitorArgTransformer()]
        [ArgumentCompleter(typeof(VCPDeviceNameCompleter))]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public VCPMonitor[] Monitor { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ArgumentCompleter(typeof(VCPCodeCompleter))]
        public byte VCPCode { get; set; }

        [Parameter(Mandatory = true, Position = 2)]
        public uint Value { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {
                try
                {
                    inputMonitor.SetVCPValue(VCPCode, Value);
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(
                        new ApiException($"Failed to set value due to error: {error.Message}", error),
                        "SetVCPValueError",
                        error.GetErrorCategory(),
                        inputMonitor));
                }
            }
        }
    }
}