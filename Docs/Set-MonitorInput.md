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

### BySource (Default)
```
Set-MonitorInput -Monitor <VCPMonitor[]> [-Source] <MonitorInputSource> [<CommonParameters>]
```

### ByValue
```
Set-MonitorInput -Monitor <VCPMonitor[]> [-Value] <UInt32> [<CommonParameters>]
```

## DESCRIPTION
Sets VCP code 0x60 (Input Select) on the specified monitor(s).

Use the `-Source` parameter for MCCS-spec values (DisplayPort1, Hdmi1, etc.).
Use the `-Value` parameter for vendor-specific inputs that aren't in the MCCS
spec (some monitors expose USB-C and other inputs at non-standard codes).

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
PS C:\> Get-Monitor -Primary | Set-MonitorInput -Value 0x1B
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
The MCCS-spec input source to switch to.

```yaml
Type: MonitorInputSource
Parameter Sets: BySource
Aliases:
Accepted values: Vga1, Vga2, Dvi1, Dvi2, Composite1, Composite2, SVideo1, SVideo2, Tuner1, Tuner2, Tuner3, Component1, Component2, Component3, DisplayPort1, DisplayPort2, Hdmi1, Hdmi2

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
A raw VCP value for input source. Useful for vendor-specific inputs
(USB-C and others) that aren't in the MCCS standard list.

```yaml
Type: UInt32
Parameter Sets: ByValue
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
