using System;

namespace MartinGC94.MonitorConfig.API.VCP
{
    /// <summary>
    /// Result type for Get-MonitorInput. CurrentSource is set when the raw
    /// CurrentValue matches one of the MCCS-spec values defined by
    /// <see cref="MonitorInputSource"/>; for vendor-specific values it is null.
    /// </summary>
    public sealed class MonitorInputInfo
    {
        public uint CurrentValue { get; }
        public MonitorInputSource? CurrentSource { get; }

        internal MonitorInputInfo(uint rawCurrentValue)
        {
            // Per MCCS 2.2a, VCP 0x60 (Input Select) is a single byte and the
            // value comes back in the SL byte of the DDC reply packet (= low
            // byte of the DWORD here). Some monitors duplicate the byte into
            // both SH and SL (so a SET of 0x11 reads back as 0x1111). Mask to
            // SL so the result compares cleanly with what was passed to Set.
            byte sl = (byte)(rawCurrentValue & 0xFF);
            CurrentValue = sl;
            CurrentSource = Enum.IsDefined(typeof(MonitorInputSource), sl)
                ? (MonitorInputSource?)(MonitorInputSource)sl
                : null;
        }
    }
}
