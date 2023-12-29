using MartinGC94.MonitorConfig.Native;
using MartinGC94.MonitorConfig.Native.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace MartinGC94.MonitorConfig.API
{
    public sealed class LogicalDisplay
    {
        public bool IsPrimary { get; private set; }
        public string DeviceName { get; private set; }
        public RECTL Bounds { get; private set; }
        public RECTL WorkingArea { get; private set; }

        private IntPtr displayHandle;

        private const uint EDD_GET_DEVICE_INTERFACE_NAME = 1;
        private const uint MONITORINFOF_PRIMARY = 1;

        private LogicalDisplay() { }

        private LogicalDisplay(MonitorInfoExW monitorInfo, IntPtr handle)
        {
            IsPrimary = monitorInfo.dwFlags == MONITORINFOF_PRIMARY;
            DeviceName = monitorInfo.szDevice;
            Bounds = monitorInfo.rcMonitor;
            WorkingArea = monitorInfo.rcWork;
            displayHandle = handle;
        }

        public override string ToString()
        {
            return DeviceName;
        }

        internal List<DisplayDevice> GetDisplayDevices()
        {
            var result = new List<DisplayDevice>();
            uint deviceIndex = 0;
            while (true)
            {
                var displayInfo = new DisplayDevice();
                displayInfo.cb = (uint)Marshal.SizeOf(displayInfo);

                if (NativeMethods.EnumDisplayDevicesW(DeviceName, deviceIndex, ref displayInfo, EDD_GET_DEVICE_INTERFACE_NAME) == 0)
                {
                    break;
                }

                result.Add(displayInfo);
                deviceIndex++;
            }

            return result;
        }

        internal PhysicalMonitor[] GetPhysicalMonitors()
        {
            if (!NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(displayHandle, out uint monitorCount))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var result = new PhysicalMonitor[monitorCount];
            if (!NativeMethods.GetPhysicalMonitorsFromHMONITOR(displayHandle, monitorCount, result))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return result;
        }

        private static MonitorInfoExW GetMonitorInfoFromHandle(IntPtr monitorHandle)
        {
            var info = new MonitorInfoExW();
            info.cbSize = (uint)Marshal.SizeOf(info);
            if (NativeMethods.GetMonitorInfoW(monitorHandle, ref info) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return info;
        }

        internal static LogicalDisplay GetPrimaryLogicalDisplay()
        {
            var result = new LogicalDisplay();
            NativeMethods.EnumMonitorsDelegate callback = (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
            {
                var info = GetMonitorInfoFromHandle(hMonitor);
                if (info.dwFlags == MONITORINFOF_PRIMARY)
                {
                    // Primary display has been found; stop enumeration.
                    result.IsPrimary = true;
                    result.DeviceName = info.szDevice;
                    result.Bounds = info.rcMonitor;
                    result.WorkingArea = info.rcWork;
                    result.displayHandle = hMonitor;

                    return false;
                }

                return true;
            };

            _ = NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero);

            return result;
        }

        internal static List<LogicalDisplay> GetMatchingLogicalDisplays(string[] DeviceNames)
        {
            WildcardPattern[] wildcards = Utils.GetWildcardPatterns(DeviceNames);
            var result = new List<LogicalDisplay>();
            NativeMethods.EnumMonitorsDelegate callback;
            if (wildcards is null)
            {
                callback = (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
                {
                    var info = GetMonitorInfoFromHandle(hMonitor);
                    result.Add(new LogicalDisplay(info, hMonitor));
                    return true;
                };
            }
            else
            {
                callback = (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
                {
                    var info = GetMonitorInfoFromHandle(hMonitor);
                    foreach (WildcardPattern pattern in wildcards)
                    {
                        if (pattern.IsMatch(info.szDevice))
                        {
                            result.Add(new LogicalDisplay(info, hMonitor));
                            break;
                        }
                    }

                    return true;
                };
            }

            _ = NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero);
            return result;
        }
    }
}
