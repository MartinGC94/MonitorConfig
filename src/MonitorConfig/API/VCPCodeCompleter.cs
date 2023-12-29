using MartinGC94.MonitorConfig.API.VCP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace MartinGC94.MonitorConfig.API
{
    internal sealed class VCPCodeCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            WildcardPattern inputPattern = WildcardPattern.Get(wordToComplete.Trim('\'', '"') + "*", WildcardOptions.IgnoreCase);
            foreach (KeyValuePair<byte, VCPFeatureDocumentation> feature in VCPFeatureDocumentation.documentationTable.OrderBy(x => x.Key))
            {
                string byteFormat = string.Format("0x{0:X2}", feature.Key);
                if (inputPattern.IsMatch(byteFormat) || inputPattern.IsMatch(feature.Value.Name))
                {
                    yield return new CompletionResult(byteFormat, $"{byteFormat} {feature.Value.Name}", CompletionResultType.ParameterValue, feature.Value.Description);
                }
            }
        }
    }
}