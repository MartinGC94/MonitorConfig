---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Get-MonitorBrightness

## SYNOPSIS
Returns brightness details (Min, Max and current value) for the specified monitor.

## SYNTAX

```
Get-MonitorBrightness -Monitor <Monitor[]> [<CommonParameters>]
```

## DESCRIPTION
Returns brightness details (Min, Max and current value) for the specified monitor.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Get-MonitorBrightness
```

Returns brightness information for all connected monitors.

## PARAMETERS

### -Monitor
The monitor(s) to return brightness details for.

```yaml
Type: Monitor[]
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### MartinGC94.MonitorConfig.API.Monitor[]

## OUTPUTS

### MartinGC94.MonitorConfig.API.BrightnessInfo

## NOTES

## RELATED LINKS
