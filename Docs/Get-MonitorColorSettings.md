---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Get-MonitorColorSettings

## SYNOPSIS
Returns color and contrast information about the specified display(s).

## SYNTAX

```
Get-MonitorColorSettings -Monitor <VCPMonitor[]> [<CommonParameters>]
```

## DESCRIPTION
Returns color and contrast information about the specified display(s).

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Get-MonitorColorSettings
```

Returns color information for all connected monitors.

## PARAMETERS

### -Monitor
The monitor(s) to return color details for.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### MartinGC94.MonitorConfig.API.VCP.VCPMonitor[]

## OUTPUTS

### MartinGC94.MonitorConfig.API.MonitorColorInfo

## NOTES

## RELATED LINKS
