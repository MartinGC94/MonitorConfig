using MartinGC94.MonitorConfig.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.API
{
    internal sealed class MonitorArgTransformer : ArgumentTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            object unwrappedInput = inputData is PSObject psObj ? psObj.BaseObject : inputData;

            if (unwrappedInput is string inputAsString)
            {
                var inputStrings = new string[] { inputAsString };
                var result = InvokeGetMonitorCmd(inputStrings);
                if (result.Count > 0)
                {
                    return result;
                }
            }
            else if (unwrappedInput is IEnumerable enumerable)
            {
                var inputStrings = new List<string>();
                var inputMonitors = new List<Monitor>();
                foreach (var item in enumerable)
                {
                    object unwrappedItem = item is PSObject psobj2 ? psobj2.BaseObject : item;
                    if (unwrappedItem is string itemAsString)
                    {
                        inputStrings.Add(itemAsString);
                    }
                    else if (unwrappedItem is Monitor itemAsMonitor)
                    {
                        inputMonitors.Add(itemAsMonitor);
                    }
                }

                if (inputStrings.Count > 0)
                {
                    var result = InvokeGetMonitorCmd(inputStrings.ToArray());
                    if (inputMonitors.Count > 0)
                    {
                        result.AddRange(inputMonitors);
                    }

                    if (result.Count > 0)
                    {
                        return result;
                    }
                }
            }

            return inputData;
        }

        private List<object> InvokeGetMonitorCmd(string[] inputStrings)
        {
            var cmd = new GetMonitorCommand()
            {
                DeviceName = inputStrings
            };

            var result = new List<object>();
            foreach (var item in cmd.Invoke())
            {
                result.Add(item);
            }

            return result;
        }
    }
}