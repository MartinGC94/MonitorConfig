using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsData.Save, "MonitorSettings")]
    public sealed class SaveMonitorSettingsCommand : Cmdlet
    {
        #region parameters
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public VCPMonitor[] Monitor { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (var inputMonitor in Monitor)
            {
                try
                {
                    inputMonitor.SaveCurrentSettings();
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(
                    new Exception($"Failed to save monitor settings due to error: {error.Message}", error),
                    "SaveSettingsError",
                    Utils.GetErrorCategory(error),
                    inputMonitor));
                }
            }
        }
    }
}