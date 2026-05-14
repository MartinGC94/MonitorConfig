using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.API.ParamAttributes
{
    /// <summary>
    /// Converts known input-source names (e.g. "Hdmi1")
    /// Anything else is passed through for PowerShell to convert to uint.
    /// </summary>
    public sealed class MonitorInputSourceArgTransformer : ArgumentTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            object unwrapped = inputData is PSObject psObj ? psObj.BaseObject : inputData;

            if (unwrapped is string inputAsString
                && Enum.TryParse(inputAsString, ignoreCase: true, out MonitorInputSource parsed)
                && Enum.IsDefined(typeof(MonitorInputSource), parsed))
            {
                return (uint)(byte)parsed;
            }

            return inputData;
        }
    }
}
