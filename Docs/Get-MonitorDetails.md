---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Get-MonitorDetails

## SYNOPSIS
Returns details of the specified monitor(s) like DDC capabilities, firmware version, usage time, etc.

## SYNTAX

```
Get-MonitorDetails -Monitor <VCPMonitor[]> [<CommonParameters>]
```

## DESCRIPTION
Returns details of the specified monitor(s) like DDC capabilities, firmware version, usage time, etc.
The DDC capabilities are based off the monitor capability string, which some manufacturers unfortunately don't always fill out correctly.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Get-MonitorDetails
```

Returns various details about all of the connected monitors.

## PARAMETERS

### -Monitor
The monitor(s) to return details for.

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

### MartinGC94.MonitorConfig.API.MonitorDetails

## NOTES

## RELATED LINKS
