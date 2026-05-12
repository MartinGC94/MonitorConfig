using MartinGC94.MonitorConfig.API.VCP;
using MartinGC94.MonitorConfig.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace MartinGC94.MonitorConfig.API
{
    /// <summary>
    /// Completer for the -Monitor parameter on cmdlets that require a VCP-capable
    /// (DDC/CI) monitor. Only suggests displays whose physical monitor is exposed
    /// as a VCPMonitor, so a Tab suggestion will always bind to a VCPMonitor[]
    /// parameter without a type-conversion error. The list of VCP device names is
    /// cached for the lifetime of the PowerShell session and invalidated when the
    /// set of logical displays changes (monitor hot-plug), so users don't pay the
    /// enumeration cost on every Tab press.
    /// </summary>
    internal sealed class VCPDeviceNameCompleter : IArgumentCompleter
    {
        private static readonly object _cacheLock = new object();
        private static string[] _cachedNames;
        private static HashSet<string> _cachedDisplaySnapshot;

        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            string cleanWordToComplete = ((wordToComplete ?? string.Empty).Trim('\'', '"')) + "*";
            var pattern = WildcardPattern.Get(cleanWordToComplete, WildcardOptions.IgnoreCase);

            string[] names;
            try
            {
                names = GetVCPDeviceNames();
            }
            catch
            {
                yield break;
            }

            foreach (string name in names)
            {
                if (pattern.IsMatch(name))
                {
                    yield return new CompletionResult(name, name, CompletionResultType.ParameterValue, name);
                }
            }
        }

        private static string[] GetVCPDeviceNames()
        {
            HashSet<string> currentSnapshot = MonitorInputCompleterHelper.SnapshotLogicalDisplayNames();

            lock (_cacheLock)
            {
                if (_cachedNames != null
                    && _cachedDisplaySnapshot != null
                    && _cachedDisplaySnapshot.SetEquals(currentSnapshot))
                {
                    return _cachedNames;
                }

                var names = new List<string>();
                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var cmd = new GetMonitorCommand()
                {
                    DeviceName = new[] { "*" }
                };
                foreach (var item in cmd.Invoke())
                {
                    object unwrapped = item is PSObject pso ? pso.BaseObject : item;
                    if (unwrapped is VCPMonitor vcp)
                    {
                        try
                        {
                            string name = vcp.LogicalDisplay?.DeviceName;
                            if (!string.IsNullOrEmpty(name) && seen.Add(name))
                            {
                                names.Add(name);
                            }
                        }
                        finally
                        {
                            // Release the physical-monitor handle; we only need the name.
                            vcp.Dispose();
                        }
                    }
                }

                _cachedNames = names.ToArray();
                _cachedDisplaySnapshot = currentSnapshot;
                return _cachedNames;
            }
        }
    }
}
