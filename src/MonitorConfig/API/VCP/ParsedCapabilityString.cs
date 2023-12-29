using System;
using System.Collections.Generic;
using System.Text;

namespace MartinGC94.MonitorConfig.API.VCP
{
    public sealed class ParsedCapabilityString
    {
        public string RawString { get; }
        public string ProtocolClass { get; private set; }
        public string DisplayType { get; private set; }
        public string DisplayModel { get; private set; }
        public ParsedVCPCode[] Commands { get; private set; }
        public ParsedVCPCode[] VCPCodes { get; private set; }
        public Version MCCSVersion { get; private set; }
        public Dictionary<byte, string> VCPNameTable { get; private set; }

        private ParsedCapabilityString(string capabilityString)
        {
            RawString = capabilityString;
        }

        /// <summary>
        /// Parses a capability string to extract key details about a display.
        /// For the sake of simplicity, and because certain displays return badly formatted strings, the parsing is very lenient.
        /// </summary>
        public static ParsedCapabilityString ParseCapabilityString(string capabilityString)
        {
            if (string.IsNullOrEmpty(capabilityString))
            {
                throw new ArgumentException("Invalid capability string", "capabilityString");
            }

            var result = new ParsedCapabilityString(capabilityString);
            var sb = new StringBuilder();

            // Capability strings usually start with '(', which we can just skip.
            for (int i = capabilityString[0] == '(' ? 1 : 0; i < capabilityString.Length; i++)
            {
                char c = capabilityString[i];
                if (c == '(')
                {
                    string parsedName = sb.ToString();
                    _ = sb.Clear();

                    try
                    {
                        if (parsedName.Equals("prot", StringComparison.OrdinalIgnoreCase))
                        {
                            result.ProtocolClass = ParseStringValue(ref i, capabilityString, sb);
                        }
                        else if (parsedName.Equals("type", StringComparison.OrdinalIgnoreCase))
                        {
                            result.DisplayType = ParseStringValue(ref i, capabilityString, sb);
                        }
                        else if (parsedName.Equals("model", StringComparison.OrdinalIgnoreCase))
                        {
                            result.DisplayModel = ParseStringValue(ref i, capabilityString, sb);
                        }
                        else if (parsedName.Equals("cmds", StringComparison.OrdinalIgnoreCase))
                        {
                            result.Commands = ParseVCPCodes(ref i, capabilityString);
                        }
                        else if (parsedName.Equals("vcp", StringComparison.OrdinalIgnoreCase))
                        {
                            result.VCPCodes = ParseVCPCodes(ref i, capabilityString);
                        }
                        else if (parsedName.Equals("mccs_ver", StringComparison.OrdinalIgnoreCase))
                        {
                            result.MCCSVersion = Version.Parse(ParseStringValue(ref i, capabilityString, sb));
                        }
                        else if (parsedName.Equals("vcpname", StringComparison.OrdinalIgnoreCase))
                        {
                            result.VCPNameTable = ParseVCPTable(ref i, capabilityString, sb);
                        }
                        else
                        {
                            SkipUnknownItem(ref i, capabilityString);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Failed to parse at section \"{parsedName}\"", e);
                    }

                    continue;
                }

                if (c == ' ')
                {
                    continue;
                }

                _ = sb.Append(c);
            }

            return result;
        }

        private static string ParseStringValue(ref int i, string capabilityString, StringBuilder sb)
        {
            for (i++; i < capabilityString.Length; i++)
            {
                char c = capabilityString[i];
                if (c == ')')
                {
                    break;
                }

                _ = sb.Append(c);
            }

            string result = sb.ToString();
            sb.Clear();
            return result;
        }

        private static ParsedVCPCode[] ParseVCPCodes(ref int i, string capabilityString)
        {
            var resultList = new List<ParsedVCPCode>();
            for (i++; i < capabilityString.Length; i++)
            {
                char c = capabilityString[i];
                if (c == ' ')
                {
                    continue;
                }

                if (c == ')')
                {
                    break;
                }

                if (c == '(')
                {
                    resultList[resultList.Count - 1].ValidValues = ParseValidVCPValues(ref i, capabilityString);
                    continue;
                }

                i++;
                char nextChar = capabilityString[i];
                var vcpCode = Convert.ToByte($"{c}{nextChar}", 16);
                var parsedCode = new ParsedVCPCode(vcpCode);
                resultList.Add(parsedCode);
            }

            return resultList.ToArray();
        }

        private static byte[] ParseValidVCPValues(ref int i, string capabilityString)
        {
            var resultList = new List<byte>();
            for (i++; i < capabilityString.Length; i++)
            {
                char c = capabilityString[i];

                if (c == ' ')
                {
                    continue;
                }

                if (c == ')')
                {
                    break;
                }

                i++;
                char nextChar = capabilityString[i];
                var newValue = Convert.ToByte($"{c}{nextChar}", 16);
                resultList.Add(newValue);
            }

            return resultList.ToArray();
        }
    
        private static Dictionary<byte, string> ParseVCPTable(ref int i, string capabilityString, StringBuilder sb)
        {
            var result = new Dictionary<byte, string>();
            byte key = default;
            for (i++; i < capabilityString.Length; i++)
            {
                char c = capabilityString[i];
                if (c == '(')
                {
                    string value = ParseStringValue(ref i, capabilityString, sb);
                    result.Add(key, value);
                    continue;
                }

                if (c == ')')
                {
                    break;
                }

                if (c == ' ')
                {
                    continue;
                }

                i++;
                char nextChar = capabilityString[i];
                key = Convert.ToByte($"{c}{nextChar}", 16);
            }

            return result;
        }
    
        private static void SkipUnknownItem(ref int i, string capabilityString)
        {
            int nestedLevel = 1;
            for (i++; i < capabilityString.Length; i++)
            {
                char c = capabilityString[i];
                if (c == '(')
                {
                    nestedLevel++;
                }
                else if (c == ')')
                {
                    nestedLevel--;
                }

                if (nestedLevel == 0)
                {
                    return;
                }
            }
        }
    }
}