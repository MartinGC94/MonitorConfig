using MartinGC94.MonitorConfig.API;
using MartinGC94.MonitorConfig.API.VCP;
using MartinGC94.MonitorConfig.API.WMI;
using MartinGC94.MonitorConfig.Native;
using MartinGC94.MonitorConfig.Native.Structs;
using Microsoft.Management.Infrastructure;
using System.Collections.Generic;
using System.Management.Automation;

namespace MartinGC94.MonitorConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "Monitor", DefaultParameterSetName = "Default")]
    [OutputType(typeof(WMIMonitor), typeof(VCPMonitor))]
    public sealed class GetMonitorCommand : Cmdlet
    {
        #region parameters
        [Parameter(ParameterSetName = "GetPrimaryMonitor")]
        public SwitchParameter Primary { get; set; }

        [Parameter(ParameterSetName = "Default", Position = 0)]
        [SupportsWildcards()]
        [ArgumentCompleter(typeof(DeviceNameCompleter))]
        public string[] DeviceName { get; set; }

        [Parameter()]
        public SwitchParameter SkipWmiCheck { get; set; }
        #endregion

        protected override void EndProcessing()
        {
            var wmiMonitorTable = SkipWmiCheck ? new Dictionary<string, CimInstance>() : WMIMonitor.GetWmiBrightnessControls();
            if (Primary)
            {
                LogicalDisplay logicalDisplay = LogicalDisplay.GetPrimaryLogicalDisplay();
                ProcessLogicalDisplay(logicalDisplay, wmiMonitorTable);
            }
            else
            {
                foreach (var item in LogicalDisplay.GetMatchingLogicalDisplays(DeviceName))
                {
                    ProcessLogicalDisplay(item, wmiMonitorTable);
                }
            }
        }

        private void ProcessLogicalDisplay(LogicalDisplay logicalDisplay, Dictionary<string, CimInstance> wmiMonitorTable)
        {
            // The logic here assumes that the output from GetPhysicalMonitors and GetDisplayDevices matches 1:1
            // Meaning that both the count and item order has to match (which from test observations seems to be true)
            // The only scenario where it has been observed not to be true is in headless scenarios, but GetPhysicalMonitors will throw in that scenario anyway.
            PhysicalMonitor[] physicalMonitors = logicalDisplay.GetPhysicalMonitors();
            List<DisplayDevice> physicalDisplayInfo = logicalDisplay.GetDisplayDevices();

            for (int i = 0; i < physicalMonitors.Length; i++)
            {
                PhysicalMonitor physicalMonitor = physicalMonitors[i];
                DisplayDevice displayDevice = physicalDisplayInfo[i];

                int instanceNameEnd = displayDevice.deviceID.LastIndexOf('#');
                string instanceName = displayDevice.deviceID.Substring(4, instanceNameEnd - 4).Replace('#', '\\');

                if (wmiMonitorTable.TryGetValue(instanceName, out CimInstance wmiInfo))
                {
                    _ = NativeMethods.DestroyPhysicalMonitor(physicalMonitor.hPhysicalMonitor);
                    WriteObject(new WMIMonitor(
                        logicalDisplay,
                        physicalMonitor.szPhysicalMonitorDescription,
                        (string)wmiInfo.CimInstanceProperties["InstanceName"].Value,
                        wmiInfo));
                }
                else
                {
                    WriteObject(new VCPMonitor(logicalDisplay, physicalMonitor.szPhysicalMonitorDescription, instanceName, physicalMonitor.hPhysicalMonitor));
                }
            }
        }
    }
}