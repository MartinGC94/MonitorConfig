using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace MartinGC94.MonitorConfig.API
{
    internal sealed class DeviceNameCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            if (wordToComplete is null)
            {
                wordToComplete = string.Empty;
            }

            string cleanWordToComplete = wordToComplete.Trim('\'', '"') + "*";
            var foundDisplays = LogicalDisplay.GetMatchingLogicalDisplays(new string[] { cleanWordToComplete });
            foreach (LogicalDisplay display in foundDisplays)
            {
                yield return new CompletionResult(display.DeviceName, display.DeviceName, CompletionResultType.ParameterValue, display.DeviceName);
            }
        }
    }
}
