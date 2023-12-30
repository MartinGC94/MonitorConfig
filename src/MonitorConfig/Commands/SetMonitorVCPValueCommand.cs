using MartinGC94.MonitorConfig.API.VCP;
using System.Management.Automation;
using System;
using MartinGC94.MonitorConfig.API;
using System.ComponentModel;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "MonitorVCPValue")]
    public sealed class SetMonitorVCPValueCommand : Cmdlet
    {
        #region parameters
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public VCPMonitor[] Monitor { get; set; }

        [Parameter(Mandatory = true)]
        [ArgumentCompleter(typeof(VCPCodeCompleter))]
        public byte VCPCode { get; set; }

        [Parameter(Mandatory = true)]
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
                        new Exception($"Failed to set value due to error: {error.Message}", error),
                        "SetVCPValueError",
                        Utils.GetErrorCategory(error),
                        inputMonitor));
                }
            }
        }
    }
}