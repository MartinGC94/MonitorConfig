using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Reset, "MonitorSettings", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public sealed class ResetMonitorSettingsCommand : Cmdlet
    {
        #region parameters
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public VCPMonitor[] Monitor { get; set; }

        [Parameter(Mandatory = true)]
        public ResetKind Kind;
        #endregion

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {
                string whatIfText = $"Will reset {Kind} settings for: {inputMonitor.FriendlyName} ({inputMonitor.LogicalDisplay.DeviceName})";
                string confirmText = $"Reset {Kind} settings for: {inputMonitor.FriendlyName} ({inputMonitor.LogicalDisplay.DeviceName})";
                if (ShouldProcess(whatIfText, confirmText, "Confirm"))
                {
                    try
                    {
                        switch (Kind)
                        {
                            case ResetKind.All:
                                inputMonitor.RestoreSettingsToFactoryDefaults();
                                break;

                            case ResetKind.Colors:
                                inputMonitor.RestoreColorSettingsToFactoryDefaults();
                                break;

                            default:
                                inputMonitor.SetVCPValue((byte)Kind, 1);
                                break;
                        }
                    }
                    catch (Win32Exception error)
                    {
                        WriteError(new ErrorRecord(
                            new Exception($"Failed to reset settings due to error: {error.Message}", error),
                            "SettingsResetError",
                            Utils.GetErrorCategory(error),
                            inputMonitor));
                    }
                }
            }
        }
    }
}