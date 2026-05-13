---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Set-MonitorInput

## SYNOPSIS
Sets the active input source on the specified monitors.

## SYNTAX

```
Set-MonitorInput -Monitor <VCPMonitor[]> [-Source] <UInt32> [<CommonParameters>]
```

## DESCRIPTION
Sets VCP code 0x60 (Input Select) on the specified monitor(s).

The `-Source` parameter accepts either a friendly name from the MCCS spec
(DisplayPort1, Hdmi1, etc.) or a raw numeric value such as `0x1B` for
vendor-specific inputs that aren't in the MCCS spec (some monitors expose
USB-C and other inputs at non-standard codes).

After the command runs, the monitor switches to the requested input. If the
calling host is connected through the input being switched away from, it will
lose the signal — the command itself succeeds either way as long as the
DDC/CI write reached the monitor.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Set-MonitorInput -Source DisplayPort1
```

Switches every active monitor to DisplayPort1.

### Example 2
```powershell
PS C:\> Get-Monitor -DeviceName \\.\DISPLAY2 | Set-MonitorInput -Source Hdmi1
```

Switches a specific monitor to HDMI1.

### Example 3
```powershell
PS C:\> Get-Monitor -Primary | Set-MonitorInput -Source 0x1B
```

Switches the primary monitor to a vendor-specific input that isn't in the
MCCS standard list (Dell P-series USB-C is `0x1B` for example).

## PARAMETERS

### -Monitor
The VCP-controlled monitor(s) to set the input on.
Internal panels that are managed via WMI cannot change input source and will
be rejected by parameter binding.

```yaml
Type: VCPMonitor[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Source
The input source to switch to. Accepts MCCS-spec friendly names
(Vga1, Vga2, Dvi1, Dvi2, Composite1, Composite2, SVideo1, SVideo2,
Tuner1, Tuner2, Tuner3, Component1, Component2, Component3,
DisplayPort1, DisplayPort2, Hdmi1, Hdmi2) or a raw numeric value
(e.g. `17`, `0x11`) for vendor-specific inputs such as USB-C.

When the -Monitor parameter is also bound, tab completion shows the
values that monitor advertises in its DDC/CI capability string.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### MartinGC94.MonitorConfig.API.VCP.VCPMonitor[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
