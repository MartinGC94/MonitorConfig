namespace MartinGC94.MonitorConfig.API.VCP
{
    /// <summary>
    /// Input source values defined by the MCCS spec for VCP code 0x60 (Input Select).
    /// Vendors often add their own values outside this list (for example USB-C
    /// inputs); use the -Value parameter on Set-MonitorInput for those.
    /// </summary>
    public enum MonitorInputSource : byte
    {
        Vga1 = 0x01,
        Vga2 = 0x02,
        Dvi1 = 0x03,
        Dvi2 = 0x04,
        Composite1 = 0x05,
        Composite2 = 0x06,
        SVideo1 = 0x07,
        SVideo2 = 0x08,
        Tuner1 = 0x09,
        Tuner2 = 0x0A,
        Tuner3 = 0x0B,
        Component1 = 0x0C,
        Component2 = 0x0D,
        Component3 = 0x0E,
        DisplayPort1 = 0x0F,
        DisplayPort2 = 0x10,
        Hdmi1 = 0x11,
        Hdmi2 = 0x12,
    }
}
