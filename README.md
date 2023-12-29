# MonitorConfig

This PowerShell module allows you to manage various monitor settings on Windows computers.  
Internal displays, like the ones found on laptops are managed with WMI and the only settings that can be adjusted are brightness related.  
External displays connected via DisplayPort, HDMI, etc. are managed with DDC/CI. Most displays support DDC/CI, but in some cases it needs to be enabled from the On Screen Display. 
For more information about DDC/CI, see: https://en.wikipedia.org/wiki/Display_Data_Channel

# Getting started

First, install the module from the PowerShell gallery: `Install-Module MonitorConfig`  
Then check the available commands in the module: `Get-Command -Module MonitorConfig`

## EXAMPLES

### List available monitors
```powershell
PS C:\> Get-Monitor

LogicalDisplay FriendlyName              InstanceName
-------------- ------------              ------------
\\.\DISPLAY1   Dell S2417DG(DisplayPort) DISPLAY\DELA0E7\5&21071cf6&0&UID12547
\\.\DISPLAY2   Dell S2417DG(DisplayPort) DISPLAY\DELA0E7\5&21071cf6&0&UID12549
\\.\DISPLAY3   Generic PnP Monitor       DISPLAY\GSM5788\5&21071cf6&0&UID12544
```

Note that the friendlyname is provided by the monitor driver, which usually also includes a color profile.

### Turn off a particular display
```powershell
PS C:\> Get-Monitor -DeviceName \\.\DISPLAY3 | Set-MonitorVCPValue -VCPCode 0xD6 -Value 4

LogicalDisplay FriendlyName              InstanceName
-------------- ------------              ------------
\\.\DISPLAY1   Dell S2417DG(DisplayPort) DISPLAY\DELA0E7\5&21071cf6&0&UID12547
\\.\DISPLAY2   Dell S2417DG(DisplayPort) DISPLAY\DELA0E7\5&21071cf6&0&UID12549
\\.\DISPLAY3   Generic PnP Monitor       DISPLAY\GSM5788\5&21071cf6&0&UID12544
```

### List all supported VCP codes for the primary display
```powershell
PS C:\> Get-Monitor -Primary | Get-MonitorVCPResponse -All

VCPCode Name                                 CurrentValue MaxValue Description
------- ----                                 ------------ -------- -----------
   0x00 Code Page                                       0        0 Returns the Code Page ID number Byte SL.
   0x02 New Control Value                               1        0 Indicates that a displays MCCS VCP Code register value has changed.
   0x03 Soft Controls                                   0        0 Allows applications running on the host to use control buttons on the display.
   0x04 Restore Factory Defaults                        0        0 Restore all factory presets including luminance / contrast, geometry, color and TV defaults.
   0x05 Restore Factory Luminance / Contras…            0        0 Restores factory defaults for luminance and contrast adjustments.
   0x08 Restore Factory Color Defaults                  0        0 Restore factory defaults for color settings.
   0x10 Luminance                                      39      100 Luminance of the image (Brightness control).
```