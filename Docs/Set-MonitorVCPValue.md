---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Set-MonitorVCPValue

## SYNOPSIS
Adjusts the value of the specified VCP code with the specified value.

## SYNTAX

```
Set-MonitorVCPValue -Monitor <VCPMonitor[]> [-VCPCode] <Byte> [-Value] <UInt32> [<CommonParameters>]
```

## DESCRIPTION
Adjusts the value of the specified VCP code with the specified value.  
This can be used to adjust any of the settings that the monitor supports, rather than the high level wrappers included in the module.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor -DeviceName \\.\DISPLAY3 | Set-MonitorVCPValue -VCPCode 0x60 -Value 3
```

Sets the VCP code 0x60 (Input select) to 3 on display3.

## PARAMETERS

### -Monitor
The monitor(s) where the VCP code + value should be set.

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
The VCP code to adjust. You can get a list of the supported VCP codes on the monitor with `Get-MonitorVCPResponse -All`.

```yaml
Type: Byte
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
The value to set. Read the DDC/CI documentation to find out what the value means for your specific VCP code.
Alternatively, you can experiment with different values and note down the results.

```yaml
Type: UInt32
Parameter Sets: (All)
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
