---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Get-Monitor

## SYNOPSIS
Gets active physical monitors for use with other commands in this module.

## SYNTAX

### Default (Default)
```
Get-Monitor [-DeviceName <String[]>] [-SkipWmiCheck] [<CommonParameters>]
```

### GetPrimaryMonitor
```
Get-Monitor [-Primary] [-SkipWmiCheck] [<CommonParameters>]
```

## DESCRIPTION
This command gets physical monitors that are currently active members of the Windows desktop.  
Depending on the exact monitor, it will be returned as either a VCPMonitor or a WMIMonitor object (typically internal laptop displays).
WMIMonitors have limited support (only support reading and setting display brightness).  
VCPMonitors can generally do anything that this module offers, though the monitor itself may lack hardware support for certain features.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor -Primary | Set-MonitorBrightness -Value 20
```

Gets the primary monitor and sets the brightness to 20.

## PARAMETERS

### -DeviceName
Specifies the Windows device name used for the logical display to find the associated physical displays.
The names look like: "\\.\DISPLAYX" where X is a number, starting from 1.  
Note that the numbers are not the same as the Windows display settings menu shows.

```yaml
Type: String[]
Parameter Sets: Default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Primary
Specifies that the command should return the physical monitors associated with the primary display.

```yaml
Type: SwitchParameter
Parameter Sets: GetPrimaryMonitor
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SkipWmiCheck
Skips checking if the found monitors are controlled by WMI rather than DDC/CI.  
Useful if you know the monitors can't be controlled with WMI and want a faster monitor lookup call, or if a monitor supports both WMI and DDC for management.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### MartinGC94.MonitorConfig.API.WMI.WMIMonitor
Returned if the monitor can be controlled through WMI. (Typically internal laptop displays)

### MartinGC94.MonitorConfig.API.VCP.VCPMonitor
Returned if the monitor cannot be controlled through WMI. DDC/CI support is not guaranteed.

## NOTES

## RELATED LINKS
