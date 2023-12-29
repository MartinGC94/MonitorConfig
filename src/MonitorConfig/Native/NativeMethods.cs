using System;
using System.Runtime.InteropServices;
using MartinGC94.MonitorConfig.Native.Structs;
using MartinGC94.MonitorConfig.Native.Enums;

namespace MartinGC94.MonitorConfig.Native
{
    internal sealed class NativeMethods
    {
        #region user32.dll
        /// <remarks>The function considers it a failure if the delegate returns false to stop the enumeration.</remarks>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern int EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        internal delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);

        /// <remarks>This function does not SetLastError on failures.</remarks>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.
        /// The function fails if iDevNum is greater than the largest device index.</returns>
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int EnumDisplayDevicesW(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int GetMonitorInfoW(IntPtr hMonitor, ref MonitorInfoExW lpmi);
        #endregion

        #region dxva2.dll
        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] PhysicalMonitor[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool DestroyPhysicalMonitor(IntPtr hMonitor);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetMonitorBrightness(IntPtr hMonitor, out uint pdwMinimumBrightness, out uint pdwCurrentBrightness, out uint pdwMaximumBrightness);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool SetMonitorBrightness(IntPtr hMonitor, uint dwNewBrightness);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool DegaussMonitor(IntPtr hMonitor);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetMonitorCapabilities(
            IntPtr hMonitor,
            out VCPMonitorCapabilities pdwMonitorCapabilities,
            out VCPMonitorColorTemperature pdwSupportedColorTemperatures);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetMonitorColorTemperature(IntPtr hMonitor, out VCPMonitorColorTemperature pctCurrentColorTemperature);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool SetMonitorColorTemperature(IntPtr hMonitor, VCPMonitorColorTemperature ctCurrentColorTemperature);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetMonitorContrast(IntPtr hMonitor, out uint pdwMinimumContrast, out uint pdwCurrentContrast, out uint pdwMaximumContrast);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool SetMonitorContrast(IntPtr hMonitor, uint dwNewContrast);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetMonitorRedGreenOrBlueDrive(
            IntPtr hMonitor,
            Color dtDriveType,
            out uint pdwMinimumDrive,
            out uint pdwCurrentDrive,
            out uint pdwMaximumDrive);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool SetMonitorRedGreenOrBlueDrive(IntPtr hMonitor, Color dtDriveType, uint dwNewDrive);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetMonitorRedGreenOrBlueGain(IntPtr hMonitor, Color gtGainType, out uint pdwMinimumGain, out uint pdwCurrentGain, out uint pdwMaximumGain);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool SetMonitorRedGreenOrBlueGain(IntPtr hMonitor, Color gtGainType, uint dwNewGain);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool RestoreMonitorFactoryColorDefaults(IntPtr hMonitor);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool RestoreMonitorFactoryDefaults(IntPtr hMonitor);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool SaveCurrentMonitorSettings(IntPtr hMonitor);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetCapabilitiesStringLength(IntPtr hMonitor, out uint pdwCapabilitiesStringLengthInCharacters);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool CapabilitiesRequestAndCapabilitiesReply(
            IntPtr hMonitor,
            [Out] byte[] pszASCIICapabilitiesString,
            uint dwCapabilitiesStringLengthInCharacters);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetVCPFeatureAndVCPFeatureReply(IntPtr hMonitor, byte bVCPCode, out VCPCodeType pvct, out uint pdwCurrentValue, out uint pdwMaximumValue);

        [DllImport("dxva2.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool SetVCPFeature(IntPtr hMonitor, byte bVCPCode, uint dwNewValue);
        #endregion
    }
}