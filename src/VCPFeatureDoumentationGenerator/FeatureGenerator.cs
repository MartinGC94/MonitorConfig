using Microsoft.CodeAnalysis;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;

namespace SourceGenerator
{
    [Generator]
    public class FeatureGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            
            var sourceBuilder = new StringBuilder(@"using System.Collections.Generic;

namespace MartinGC94.MonitorConfig.API.VCP
{
    internal partial class VCPFeatureDocumentation
    {
        internal static Dictionary<byte, VCPFeatureDocumentation> documentationTable = new Dictionary<byte, VCPFeatureDocumentation>()
        {");

            string[] csvLines = File.ReadAllLines(context.AdditionalFiles[0].Path, Encoding.UTF8);
            for (int i = 1; i < csvLines.Length; i++)
            {
                List<string> lineContent = ParseCsvLine(csvLines[i]);
                if (lineContent is null)
                {
                    continue;
                }

                string code = lineContent[0];
                string name = lineContent[1];
                string description = lineContent[3];
                string access;
                switch (lineContent[2])
                {
                    case "RW":
                        access = "VCPCodeAccess.ReadAndWrite";
                        break;

                    case "RO":
                        access = "VCPCodeAccess.ReadOnly";
                        break;

                    case "WO":
                        access = "VCPCodeAccess.WriteOnly";
                        break;

                    default:
                        throw new Exception($"Unexpected access value {lineContent[2]}");
                }

                string lineEntry = $"{{{code}, new VCPFeatureDocumentation(\"{name}\", {access}, \"{description}\") }},";
                _ = sourceBuilder.AppendLine(lineEntry);
            }

            sourceBuilder.Length -= Environment.NewLine.Length + 1;
            _ = sourceBuilder.Append(@"};}}");
            context.AddSource("VCPFeatureDocumentation.g.cs", sourceBuilder.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    
        private static List<string> ParseCsvLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            line = line.Trim();
            var result = new List<string>();
            var columnContent = new StringBuilder();
            bool inQuote = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == ',')
                {
                    if (inQuote)
                    {
                        _ = columnContent.Append(c);
                    }
                    else
                    {
                        result.Add(columnContent.ToString());
                        columnContent.Length = 0;
                    }
                }
                else if (c == '"')
                {
                    if (inQuote)
                    {
                        if (i == line.Length - 1)
                        {
                            result.Add(columnContent.ToString());
                            columnContent.Length = 0;
                            break;
                        }

                        char nextC = line[i + 1];
                        if (nextC == '"')
                        {
                            _ = columnContent.Append('"');
                            i++;
                        }
                        else if (nextC == ',')
                        {
                            inQuote = false;
                            result.Add(columnContent.ToString());
                            columnContent.Length = 0;
                            i++;
                        }
                        else
                        {
                            throw new ArgumentException("This line is not a valid CSV line");
                        }
                    }
                    else
                    {
                        inQuote = true;
                    }
                }
                else
                {
                    _ = columnContent.Append(c);
                }
            }

            if (columnContent.Length != 0)
            {
                result.Add(columnContent.ToString());
            }

            return result;
        }
    }
}