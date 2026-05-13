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
    /// Completer for Set-MonitorInput -Source. When the -Monitor parameter is
    /// bound (by object or device-name string), suggestions come from the
    /// monitor's advertised values for VCP 0x60. MCCS-spec values are offered
    /// as friendly names (Hdmi1, DisplayPort1, etc.) and vendor-specific
    /// values (e.g. USB-C) are offered as hex. Falls back to the full MCCS
    /// list when no monitor is bound.
    /// </summary>
    internal sealed class MonitorInputSourceCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var pattern = WildcardPattern.Get((wordToComplete ?? string.Empty).Trim('\'', '"') + "*", WildcardOptions.IgnoreCase);
            byte[] supported = MonitorInputCompleterHelper.TryGetSupportedValuesFromBoundMonitor(fakeBoundParameters);

            if (supported != null && supported.Length > 0)
            {
                foreach (byte value in supported)
                {
                    foreach (var result in BuildCompletions(value, pattern))
                    {
                        yield return result;
                    }
                }

                yield break;
            }

            foreach (MonitorInputSource source in (MonitorInputSource[])Enum.GetValues(typeof(MonitorInputSource)))
            {
                foreach (var result in BuildCompletions((byte)source, pattern))
                {
                    yield return result;
                }
            }
        }

        private static IEnumerable<CompletionResult> BuildCompletions(byte value, WildcardPattern pattern)
        {
            string hex = string.Format(CultureInfo.InvariantCulture, "0x{0:X2}", value);
            string dec = value.ToString(CultureInfo.InvariantCulture);

            if (Enum.IsDefined(typeof(MonitorInputSource), value))
            {
                string name = ((MonitorInputSource)value).ToString();
                string tooltip = string.Format(CultureInfo.InvariantCulture, "{0} ({1}) - {2}", hex, dec, name);
                if (pattern.IsMatch(name) || pattern.IsMatch(hex) || pattern.IsMatch(dec))
                {
                    yield return new CompletionResult(name, name, CompletionResultType.ParameterValue, tooltip);
                }
            }
            else
            {
                string tooltip = string.Format(CultureInfo.InvariantCulture, "{0} ({1}) - vendor-specific", hex, dec);
                if (pattern.IsMatch(hex) || pattern.IsMatch(dec))
                {
                    yield return new CompletionResult(hex, hex, CompletionResultType.ParameterValue, tooltip);
                }
            }
        }
    }

    internal static class MonitorInputCompleterHelper
    {
        public static byte[] TryGetSupportedValues(VCPMonitor monitor)
        {
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
                // Best-effort lookup; never throw out of completion or out of the get-cmdlet's discovery path.
            }

            return null;
        }

        private static readonly object _capsCacheLock = new object();
        private static readonly Dictionary<string, byte[]> _capsCache = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
        private static HashSet<string> _capsCacheDisplaySnapshot;

        /// <summary>
        /// Returns the current set of logical-display device names via a cheap Win32
        /// enumeration. Used as a fingerprint to detect monitor hot-plug between cache
        /// reads — any add/remove changes the set, so cache entries get invalidated
        /// automatically without a wall-clock TTL.
        /// </summary>
        internal static HashSet<string> SnapshotLogicalDisplayNames()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in LogicalDisplay.GetMatchingLogicalDisplays(null))
            {
                if (!string.IsNullOrEmpty(d.DeviceName))
                {
                    set.Add(d.DeviceName);
                }
            }
            return set;
        }

        /// <summary>
        /// Completer-side variant: caches the parsed VCP 0x60 ValidValues by a key derived
        /// from the bound -Monitor so repeated Tab presses don't reissue DDC/CI capability
        /// reads. The cache lives for the lifetime of the PowerShell session and is
        /// invalidated when the set of logical displays changes (monitor hot-plug). The
        /// cmdlet path uses the uncached <see cref="TryGetSupportedValues"/>.
        /// </summary>
        public static byte[] TryGetSupportedValuesFromBoundMonitor(IDictionary fakeBoundParameters)
        {
            string cacheKey = TryBuildCacheKey(fakeBoundParameters);
            if (cacheKey == null)
            {
                return null;
            }

            HashSet<string> currentSnapshot = SnapshotLogicalDisplayNames();

            lock (_capsCacheLock)
            {
                if (_capsCacheDisplaySnapshot != null && !_capsCacheDisplaySnapshot.SetEquals(currentSnapshot))
                {
                    _capsCache.Clear();
                }
                _capsCacheDisplaySnapshot = currentSnapshot;

                if (_capsCache.TryGetValue(cacheKey, out byte[] cached))
                {
                    return cached;
                }
            }

            byte[] values = TryGetSupportedValues(ResolveFirstVCPMonitor(fakeBoundParameters));

            lock (_capsCacheLock)
            {
                _capsCache[cacheKey] = values;
            }
            return values;
        }

        private static string TryBuildCacheKey(IDictionary fakeBoundParameters)
        {
            if (fakeBoundParameters == null || !fakeBoundParameters.Contains("Monitor"))
            {
                return null;
            }

            object raw = fakeBoundParameters["Monitor"];
            object unwrapped = raw is PSObject pso ? pso.BaseObject : raw;

            if (unwrapped is VCPMonitor direct && !string.IsNullOrEmpty(direct.InstanceName))
            {
                return "vcp:" + direct.InstanceName;
            }

            if (unwrapped is string s && !string.IsNullOrEmpty(s))
            {
                return "str:" + s;
            }

            if (unwrapped is IEnumerable enumerable && !(unwrapped is string))
            {
                foreach (var item in enumerable)
                {
                    object unwrappedItem = item is PSObject p ? p.BaseObject : item;
                    if (unwrappedItem is VCPMonitor v && !string.IsNullOrEmpty(v.InstanceName))
                    {
                        return "vcp:" + v.InstanceName;
                    }
                    if (unwrappedItem is string str && !string.IsNullOrEmpty(str))
                    {
                        return "str:" + str;
                    }
                }
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
                var cmd = new GetMonitorCommand()
                {
                    DeviceName = deviceNames.ToArray()
                };
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
