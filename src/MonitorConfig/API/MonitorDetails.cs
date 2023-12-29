using MartinGC94.MonitorConfig.API.VCP;
using System;
using System.Collections.Generic;

namespace MartinGC94.MonitorConfig.API
{
    public sealed class MonitorDetails
    {
        public ParsedCapabilityString CapabilityInfo { get; internal set; }
        public double RefreshRate { get; internal set; }
        public TimeSpan DisplayUsageTime { get; internal set; }
        public string ControllerManufacturer { get; internal set; }
        public uint ControllerID { get; internal set; }
        public Version FirmwareVersion { get; internal set; }

        internal MonitorDetails() { }

        internal static string GetDisplayControllerOEM(uint id)
        {
            if (DisplayControllerOEMTable.TryGetValue(id, out string result))
            {
                return result;
            }

            return "Unknown";
        }

        private static Dictionary<uint, string> DisplayControllerOEMTable = new Dictionary<uint, string>()
        {
            {0x00, "Reserved"},
            {0x01, "Conexant"},
            {0x02, "Genesis Microchip"},
            {0x03, "Macronix"},
            {0x04, "IDT (Integrated Device Technology)"},
            {0x05, "Mstar Semiconductor"},
            {0x06, "Myson"},
            {0x07, "Philips"},
            {0x08, "PixelWorks"},
            {0x09, "RealTek Semiconductor"},
            {0x0A, "Sage"},
            {0x0B, "Silicon Image"},
            {0x0C, "SmartASIC"},
            {0x0D, "STMicroelectronics"},
            {0x0E, "Topro"},
            {0x0F, "Trumpion"},
            {0x10, "Welltrend"},
            {0x11, "Samsung"},
            {0x12, "Novatek Microelectronics"},
            {0x13, "STK"},
            {0x14, "Silicon Optix Inc."},
            {0x15, "Texas Instruments"},
            {0x16, "Analogix Semiconductor"},
            {0x17, "Quantum Data"},
            {0x18, "NXP Semiconductors"},
            {0x19, "Chrontel"},
            {0x1A, "Parade Technologies"},
            {0x1B, "THine Electronics"},
            {0x1C, "Trident"},
            {0x1D, "Micronas"},
            {0xFF, "Undefined"}
        };
    }
}