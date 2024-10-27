---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Save-MonitorSettings

## SYNOPSIS
Issues a command to save the current display settings on the specified monitor.

## SYNTAX

```
Save-MonitorSettings -Monitor <VCPMonitor[]> [<CommonParameters>]
```

## DESCRIPTION
Issues a command to save the current display settings on the specified monitor.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Save-MonitorSettings
```

Save the current settings for all connected monitors.

## PARAMETERS

### -Monitor
The monitor(s) where the modified settings should be saved.

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

### System.Object
## NOTES

## RELATED LINKS
