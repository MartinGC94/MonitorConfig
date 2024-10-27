---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Reset-MonitorSettings

## SYNOPSIS
Resets various monitor settings to their factory defaults.

## SYNTAX

```
Reset-MonitorSettings -Monitor <VCPMonitor[]> [-Kind] <ResetKind> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Resets various monitor settings to their factory defaults.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor -DeviceName \\.\DISPLAY1 | Reset-MonitorSettings -Kind Colors
```

Resets all color settings for Display1.

## PARAMETERS

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Kind
The settings that should be reset.

```yaml
Type: ResetKind
Parameter Sets: (All)
Aliases:
Accepted values: All, BrightnessAndContrast, Geometry, Colors, TVDefaults

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Monitor
The monitor(s) to reset settings on.

```yaml
Type: VCPMonitor[]
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
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
