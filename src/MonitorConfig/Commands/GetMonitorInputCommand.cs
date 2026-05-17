using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.ParamAttributes;
using MartinGC94.MonitorConfig.API.VCP;
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

        [Parameter]
        public SwitchParameter SkipSupportedValues { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            foreach (VCPMonitor inputMonitor in Monitor)
            {

                byte[] possibleValues = SkipSupportedValues
                    ? null
                    : MonitorInputCompleterHelper.TryGetSupportedValues(inputMonitor);
                MonitorInputInfo inputInfo;
                try
                {
                    inputInfo = inputMonitor.GetInputInfo(possibleValues);
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(
                        new ApiException($"Failed to get input source due to error: {error.Message}", error),
                        "GetInputError",
                        error.GetErrorCategory(),
                        inputMonitor));
                    continue;
                }

                WriteObject(inputInfo);
            }
        }
    }
}
