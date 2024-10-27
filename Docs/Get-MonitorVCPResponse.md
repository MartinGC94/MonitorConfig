---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Get-MonitorVCPResponse

## SYNOPSIS
Query the specified monitors for the specified VCP codes.

## SYNTAX

### SpecifiedCode
```
Get-MonitorVCPResponse -Monitor <VCPMonitor[]> -VCPCode <Byte[]> [<CommonParameters>]
```

### All
```
Get-MonitorVCPResponse -Monitor <VCPMonitor[]> [-All] [<CommonParameters>]
```

## DESCRIPTION
This command lets you query the specified monitors for the current value and max possible value for a specified VCP code.  
Alternatively, you can query the monitor for all supported codes with the -All parameter.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor -DeviceName \\.\DISPLAY1 | Get-MonitorVCPResponse -All
```

Returns all supported VCP codes by for display1.

## PARAMETERS

### -All
Queries the monitor for all possible VCP codes from 0-255 and returns the ones that are supported, along with their value.

```yaml
Type: SwitchParameter
Parameter Sets: All
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Monitor
The monitor(s) to return VCP code details for.

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

### -VCPCode
The VCP codes (0-255) to query.

```yaml
Type: Byte[]
Parameter Sets: SpecifiedCode
Aliases:

Required: True
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

### MartinGC94.MonitorConfig.API.VCP.VCPFeatureResponse

## NOTES

## RELATED LINKS
