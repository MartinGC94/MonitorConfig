---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Get-MonitorInput

## SYNOPSIS
Gets the currently active input source on the specified monitors.

## SYNTAX

```
Get-MonitorInput -Monitor <VCPMonitor[]> [<CommonParameters>]
```

## DESCRIPTION
Reads VCP code 0x60 (Input Select) on the specified monitor(s) and returns a
`MonitorInputInfo` object with both the raw value and, when it matches an
MCCS-spec input, the typed `CurrentSource`. Vendor-specific inputs (USB-C and
similar) report the raw value with `CurrentSource` left null.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Get-MonitorInput

CurrentValue CurrentSource
------------ -------------
          17 Hdmi1
          17 Hdmi1
```

### Example 2
```powershell
PS C:\> Get-Monitor -Primary | Get-MonitorInput | Select-Object -ExpandProperty CurrentSource
Hdmi1
```

## PARAMETERS

### -Monitor
The VCP-controlled monitor(s) to read the input source from.
Internal panels that are managed via WMI don't expose an input source.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### MartinGC94.MonitorConfig.API.VCP.VCPMonitor[]

## OUTPUTS

### MartinGC94.MonitorConfig.API.VCP.MonitorInputInfo
## NOTES

## RELATED LINKS
