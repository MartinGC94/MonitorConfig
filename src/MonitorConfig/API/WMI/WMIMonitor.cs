using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MartinGC94.MonitorConfig.API.WMI
{
    public sealed class WMIMonitor : Monitor
    {
        private readonly CimInstance brightnessControl;
        internal const string MonitorNamespace = "root/WMI";
        internal const string BrightnessQuery = "SELECT CurrentBrightness FROM WmiMonitorBrightness WHERE InstanceName = '{0}'";
        internal const string BrightnessInfoQuery = "SELECT CurrentBrightness,Level FROM WmiMonitorBrightness WHERE InstanceName = '{0}'";

        internal WMIMonitor(LogicalDisplay logicalDisplay, string friendlyName, string instanceName, CimInstance wmiBrightnessControl) : base(logicalDisplay, friendlyName, instanceName)
        {
            brightnessControl = wmiBrightnessControl;
        }

        private void InvokeCimMethod(string methodName, CimMethodParametersCollection parameters)
        {
            var cimSession = CimSession.Create(computerName: null);
            try
            {
                var res = cimSession.InvokeMethod(brightnessControl, methodName, parameters);
            }
            finally
            {
                cimSession.Dispose();
            }
        }

        public override uint GetBrightnessLevel()
        {
            var cimSession = CimSession.Create(computerName: null);
            try
            {
                CimInstance instance = cimSession.QueryInstances(MonitorNamespace, "WQL", string.Format(BrightnessQuery, InstanceName.Replace(@"\", @"\\"))).First();
                return Convert.ToUInt32(instance.CimInstanceProperties["CurrentBrightness"].Value);
            }
            finally
            {
                cimSession.Dispose();
            }
        }

        public override void SetBrightnessLevel(uint level)
        {
            SetBrightnessLevel((byte)level, 1);
        }

        public override BrightnessInfo GetBrightnessInfo()
        {
            var cimSession = CimSession.Create(computerName: null);
            try
            {
                CimInstance instance = cimSession.QueryInstances(MonitorNamespace, "WQL", string.Format(BrightnessInfoQuery, InstanceName.Replace(@"\", @"\\"))).First();
                var brightness = Convert.ToUInt32(instance.CimInstanceProperties["CurrentBrightness"].Value);
                var levels = (byte[])instance.CimInstanceProperties["Level"].Value;
                return new BrightnessInfo(brightness, Convert.ToUInt32(levels[0]), Convert.ToUInt32(levels[levels.Length - 1]));
            }
            finally
            {
                cimSession.Dispose();
            }
        }

        public void SetBrightnessLevel(byte level, uint timeout)
        {
            var paramsToSet = new CimMethodParametersCollection()
            {
                CimMethodParameter.Create("Timeout", timeout, CimFlags.None),
                CimMethodParameter.Create("Brightness", level, CimFlags.None)
            };
            InvokeCimMethod("WmiSetBrightness", paramsToSet);
        }

        public void RevertToPolicyBrightness()
        {
            InvokeCimMethod("WmiRevertToPolicyBrightness", parameters: null);
        }

        public void SetAmbientLightSensorState(bool enabled)
        {
            var paramsToSet = new CimMethodParametersCollection()
            {
                CimMethodParameter.Create("State", enabled, CimFlags.None)
            };
            InvokeCimMethod("WmiSetALSBrightnessState", paramsToSet);
        }

        public void SetAmbientLightSensorBrightnessLevel(byte level)
        {
            var paramsToSet = new CimMethodParametersCollection()
            {
                CimMethodParameter.Create("Brightness", level, CimFlags.None)
            };
            InvokeCimMethod("WmiSetALSBrightness", paramsToSet);
        }

        internal static Dictionary<string, CimInstance> GetWmiBrightnessControls()
        {
            // Capacity is set to 1 because most computers will have 1 display that can be controlled by WMI (Internal laptop display)
            var result = new Dictionary<string, CimInstance>(1);
            CimSession cimSession = null;
            try
            {
                cimSession = CimSession.Create(computerName: null);
                foreach (CimInstance wmiMonitor in cimSession.EnumerateInstances(MonitorNamespace, "WmiMonitorBrightnessMethods"))
                {
                    string instanceName = (string)wmiMonitor.CimInstanceProperties["InstanceName"].Value;
                    // The instance name from WMI includes "_0" which is presumably some sort of index
                    // I can't find any info about it though, so I'll assume it's always 0.
                    // This suffix is removed from the key so it can be found with the instance name from GetDisplayDevices()
                    if (instanceName.EndsWith("_0"))
                    {
                        instanceName = instanceName.Remove(instanceName.Length - 2);
                    }

                    result.Add(instanceName, wmiMonitor);
                }
            }
            catch (CimException)
            {
                // The computer doesn't have any monitors that can be controlled with WMI
            }
            finally
            {
                cimSession?.Dispose();
            }

            return result;
        }
    }
}