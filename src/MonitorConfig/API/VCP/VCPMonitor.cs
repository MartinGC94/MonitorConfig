using MartinGC94.MonitorConfig.Native;
using MartinGC94.MonitorConfig.Native.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace MartinGC94.MonitorConfig.API.VCP
{
    public sealed class VCPMonitor : Monitor, IDisposable
    {
        private bool disposedValue;
        private readonly IntPtr physicalMonitorHandle;

        // Errors that we expect to see when scanning for valid VCP codes
        private const int ERROR_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED = -1071241852;
        private const int ERROR_GRAPHICS_I2C_ERROR_TRANSMITTING_DATA = -1071241854;
        // Not expected, but bad implementations of individual codes should not stop the scan
        private const int ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH = -1071241846;

        internal VCPMonitor(LogicalDisplay logicalDisplay, string friendlyName, string instanceName, IntPtr handle) : base(logicalDisplay, friendlyName, instanceName)
        {
            physicalMonitorHandle = handle;
        }

        ~VCPMonitor()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _ = NativeMethods.DestroyPhysicalMonitor(physicalMonitorHandle);
                disposedValue = true;
            }
        }

        public override uint GetBrightnessLevel()
        {
            if (!NativeMethods.GetMonitorBrightness(physicalMonitorHandle, out _, out uint brightnessLevel, out _))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return brightnessLevel;
        }

        public override void SetBrightnessLevel(uint level)
        {
            if (!NativeMethods.SetMonitorBrightness(physicalMonitorHandle, level))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public override BrightnessInfo GetBrightnessInfo()
        {
            if (!NativeMethods.GetMonitorBrightness(physicalMonitorHandle, out uint minBrightness, out uint brightnessLevel, out uint maxBrightness))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return new BrightnessInfo(brightnessLevel, minBrightness, maxBrightness);
        }

        public void DegaussMonitor()
        {
            if (!NativeMethods.DegaussMonitor(physicalMonitorHandle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public VCPMonitorColorTemperature GetMonitorColorTemperature()
        {
            if (!NativeMethods.GetMonitorColorTemperature(physicalMonitorHandle, out VCPMonitorColorTemperature colorTemp))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return colorTemp;
        }

        public void SetMonitorColorTemperature(VCPMonitorColorTemperature newTemp)
        {
            if (!NativeMethods.SetMonitorColorTemperature(physicalMonitorHandle, newTemp))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public ContrastInfo GetContrastInfo()
        {
            if (!NativeMethods.GetMonitorContrast(physicalMonitorHandle, out uint minContrast, out uint ContrastLevel, out uint maxContrast))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return new ContrastInfo(ContrastLevel, minContrast, maxContrast);
        }

        public void SetContrastLevel(uint contrastValue)
        {
            if (!NativeMethods.SetMonitorContrast(physicalMonitorHandle, contrastValue))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public ColorValue GetRGBDriveInfo(Color color)
        {
            if (!NativeMethods.GetMonitorRedGreenOrBlueDrive(physicalMonitorHandle, color, out uint min, out uint current, out uint max))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return new ColorValue(current, min, max);
        }

        public void SetRGBDrive(Color color, uint value)
        {
            if (!NativeMethods.SetMonitorRedGreenOrBlueDrive(physicalMonitorHandle, color, value))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public ColorValue GetRGBGainInfo(Color color)
        {
            if (!NativeMethods.GetMonitorRedGreenOrBlueGain(physicalMonitorHandle, color, out uint min, out uint current, out uint max))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return new ColorValue(current, min, max);
        }

        public void SetRGBGain(Color color, uint value)
        {
            if (!NativeMethods.SetMonitorRedGreenOrBlueGain(physicalMonitorHandle, color, value))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void RestoreColorSettingsToFactoryDefaults()
        {
            if (!NativeMethods.RestoreMonitorFactoryColorDefaults(physicalMonitorHandle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void RestoreSettingsToFactoryDefaults()
        {
            if (!NativeMethods.RestoreMonitorFactoryDefaults(physicalMonitorHandle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void SaveCurrentSettings()
        {
            if (!NativeMethods.SaveCurrentMonitorSettings(physicalMonitorHandle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public string GetCapabilitiesString()
        {
            if (!NativeMethods.GetCapabilitiesStringLength(physicalMonitorHandle, out uint stringLength))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var resBuilder = new byte[stringLength];
            if (!NativeMethods.CapabilitiesRequestAndCapabilitiesReply(physicalMonitorHandle, resBuilder, stringLength))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return Encoding.ASCII.GetString(resBuilder, 0, (int)(stringLength - 1)).Trim('\0');
        }

        public VCPFeatureResponse GetVCPFeatureResponse(byte vcpCode)
        {
            if (!NativeMethods.GetVCPFeatureAndVCPFeatureReply(physicalMonitorHandle, vcpCode, out VCPCodeType vcpType, out uint value, out uint maxValue))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return new VCPFeatureResponse(vcpCode, vcpType, value, maxValue);
        }

        public IEnumerable<VCPFeatureResponse> ScanVCPFeatures()
        {
            byte b = byte.MinValue;
            do
            {
                if (NativeMethods.GetVCPFeatureAndVCPFeatureReply(physicalMonitorHandle, b, out VCPCodeType vcpType, out uint value, out uint maxValue))
                {
                    yield return new VCPFeatureResponse(b, vcpType, value, maxValue);
                }
                else
                {
                    int errorId = Marshal.GetLastWin32Error();
                    if (errorId != ERROR_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED &&
                        errorId != ERROR_GRAPHICS_I2C_ERROR_TRANSMITTING_DATA &&
                        errorId != ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH)
                    {
                        throw new Win32Exception(errorId);
                    }
                }

            } while (b++ < byte.MaxValue);
        }

        public void SetVCPValue(byte vcpCode, uint value)
        {
            if (!NativeMethods.SetVCPFeature(physicalMonitorHandle, vcpCode, value))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}