using System;
using System.Globalization;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.API.VCP
{
    /// <summary>
    /// Converts known input-source names (e.g. "Hdmi1") and hex strings
    /// (e.g. "0x1B") to a uint VCP value. Anything else is passed through
    /// for PowerShell to convert to uint.
    /// </summary>
    internal sealed class MonitorInputSourceArgTransformer : ArgumentTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            object unwrapped = inputData is PSObject psObj ? psObj.BaseObject : inputData;

            if (unwrapped is string inputAsString)
            {
                string trimmed = inputAsString.Trim();
                if (Enum.TryParse(trimmed, true, out MonitorInputSource parsed)
                    && Enum.IsDefined(typeof(MonitorInputSource), parsed))
                {
                    return (uint)(byte)parsed;
                }

                if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                    && uint.TryParse(trimmed.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint hex))
                {
                    return hex;
                }
            }

            return inputData;
        }
    }
}
