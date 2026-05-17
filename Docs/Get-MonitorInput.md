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
Get-MonitorInput [-Monitor] <VCPMonitor[]> [-SkipSupportedValues] [<CommonParameters>]
```

## DESCRIPTION
Reads VCP code 0x60 (Input Select) on the specified monitor(s) and returns a `MonitorInputInfo` object with:

- `CurrentValue` - the raw VCP value currently active.

- `CurrentSource` - the typed `MonitorInputSource` when the raw value matches an MCCS-spec input; null for vendor-specific values (USB-C and similar).

- `PossibleValues` - the raw byte values the monitor advertises for VCP 0x60 in its DDC/CI capability string. Null when the capability string is unavailable or doesn't list VCP 0x60.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Get-MonitorInput

CurrentValue CurrentSource PossibleValues
------------ ------------- --------------
          17 Hdmi1         {15, 17, 27}
          17 Hdmi1         {15, 17, 27}
```

### Example 2
```powershell
PS C:\> Get-Monitor -Primary | Get-MonitorInput | Select-Object -ExpandProperty CurrentSource
Hdmi1
```

### Example 3
```powershell
PS C:\> Get-Monitor -Primary | Get-MonitorInput | ForEach-Object {
>>     $_.PossibleValues | ForEach-Object {
>>         if ([Enum]::IsDefined([MartinGC94.MonitorConfig.API.VCP.MonitorInputSource], [byte]$_)) {
>>             [MartinGC94.MonitorConfig.API.VCP.MonitorInputSource]$_
>>         } else {
>>             '0x{0:X2}' -f $_
>>         }
>>     }
>> }
DisplayPort1
Hdmi1
0x1B
```

Maps `PossibleValues` to friendly names where possible, falling back to hex for
vendor-specific values.

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

### -SkipSupportedValues
Skips getting the supported values for input switching.

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

### MartinGC94.MonitorConfig.API.VCP.VCPMonitor[]

## OUTPUTS

### MartinGC94.MonitorConfig.API.VCP.MonitorInputInfo
## NOTES

## RELATED LINKS
