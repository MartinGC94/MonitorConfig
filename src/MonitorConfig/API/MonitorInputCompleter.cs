using MartinGC94.MonitorConfig.API.VCP;
using MartinGC94.MonitorConfig.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace MartinGC94.MonitorConfig.API
{
    /// <summary>
    /// Completer for Set-MonitorInput -Source. When the -Monitor parameter
    /// is bound (by object or device-name string), the suggestions are
    /// filtered to MCCS-spec values that monitor advertises in its
    /// capability string. Falls back to the full enum if the monitor is
    /// unknown or advertises no MCCS-mapped value.
    /// </summary>
    internal sealed class MonitorInputSourceCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var pattern = WildcardPattern.Get((wordToComplete ?? string.Empty).Trim('\'', '"') + "*", WildcardOptions.IgnoreCase);
            byte[] supported = MonitorInputCompleterHelper.GetSupportedValuesFromBoundMonitor(fakeBoundParameters);

            var allSources = (MonitorInputSource[])Enum.GetValues(typeof(MonitorInputSource));
            MonitorInputSource[] suggestions = allSources;
            if (supported != null && supported.Length > 0)
            {
                var supportedSet = new HashSet<byte>(supported);
                var filtered = Array.FindAll(allSources, s => supportedSet.Contains((byte)s));
                if (filtered.Length > 0)
                {
                    suggestions = filtered;
                }
            }

            foreach (var source in suggestions)
            {
                string name = source.ToString();
                if (pattern.IsMatch(name))
                {
                    string tooltip = string.Format(CultureInfo.InvariantCulture, "0x{0:X2} ({1})", (byte)source, name);
                    yield return new CompletionResult(name, name, CompletionResultType.ParameterValue, tooltip);
                }
            }
        }
    }

    /// <summary>
    /// Completer for Set-MonitorInput -Value. When the -Monitor parameter is
    /// bound, suggestions come from the monitor's advertised values for VCP
    /// 0x60 (this is the right list for discovering vendor-specific inputs
    /// such as USB-C, which is not in the MCCS spec). Falls back to the
    /// MCCS values when the monitor is unknown.
    /// </summary>
    internal sealed class MonitorInputValueCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var pattern = WildcardPattern.Get((wordToComplete ?? string.Empty).Trim('\'', '"') + "*", WildcardOptions.IgnoreCase);
            byte[] supported = MonitorInputCompleterHelper.GetSupportedValuesFromBoundMonitor(fakeBoundParameters);

            byte[] toSuggest;
            if (supported != null && supported.Length > 0)
            {
                toSuggest = supported;
            }
            else
            {
                var allSources = (MonitorInputSource[])Enum.GetValues(typeof(MonitorInputSource));
                toSuggest = Array.ConvertAll(allSources, s => (byte)s);
            }

            foreach (byte value in toSuggest)
            {
                string asHex = string.Format(CultureInfo.InvariantCulture, "0x{0:X2}", value);
                string asDec = value.ToString(CultureInfo.InvariantCulture);
                string sourceName = Enum.IsDefined(typeof(MonitorInputSource), value)
                    ? ((MonitorInputSource)value).ToString()
                    : "vendor-specific";
                string tooltip = string.Format(CultureInfo.InvariantCulture, "{0} ({1}) - {2}", asHex, asDec, sourceName);

                if (pattern.IsMatch(asHex) || pattern.IsMatch(asDec) || pattern.IsMatch(sourceName))
                {
                    yield return new CompletionResult(asHex, tooltip, CompletionResultType.ParameterValue, tooltip);
                }
            }
        }
    }

    internal static class MonitorInputCompleterHelper
    {
        public static byte[] GetSupportedValuesFromBoundMonitor(IDictionary fakeBoundParameters)
        {
            VCPMonitor monitor = ResolveFirstVCPMonitor(fakeBoundParameters);
            if (monitor == null)
            {
                return null;
            }

            try
            {
                var caps = ParsedCapabilityString.ParseCapabilityString(monitor.GetCapabilitiesString());
                if (caps.VCPCodes == null)
                {
                    return null;
                }
                foreach (var vcp in caps.VCPCodes)
                {
                    if (vcp.VCPCode == KnownVcpCodes.InputSelect)
                    {
                        return vcp.ValidValues;
                    }
                }
            }
            catch
            {
                // Best-effort completion; never throw out of CompleteArgument.
            }

            return null;
        }

        private static VCPMonitor ResolveFirstVCPMonitor(IDictionary fakeBoundParameters)
        {
            if (fakeBoundParameters == null || !fakeBoundParameters.Contains("Monitor"))
            {
                return null;
            }

            object raw = fakeBoundParameters["Monitor"];
            object unwrapped = raw is PSObject pso ? pso.BaseObject : raw;

            if (unwrapped is VCPMonitor direct)
            {
                return direct;
            }

            var deviceNames = new List<string>();
            if (unwrapped is string s)
            {
                deviceNames.Add(s);
            }
            else if (unwrapped is IEnumerable enumerable && !(unwrapped is string))
            {
                foreach (var item in enumerable)
                {
                    object unwrappedItem = item is PSObject p ? p.BaseObject : item;
                    if (unwrappedItem is VCPMonitor vcpItem)
                    {
                        return vcpItem;
                    }
                    if (unwrappedItem is string str)
                    {
                        deviceNames.Add(str);
                    }
                }
            }

            if (deviceNames.Count == 0)
            {
                return null;
            }

            try
            {
                var cmd = new GetMonitorCommand { DeviceName = deviceNames.ToArray() };
                foreach (var item in cmd.Invoke())
                {
                    object unwrappedItem = item is PSObject p ? p.BaseObject : item;
                    if (unwrappedItem is VCPMonitor vcp)
                    {
                        return vcp;
                    }
                }
            }
            catch
            {
                // Best-effort.
            }

            return null;
        }
    }
}
