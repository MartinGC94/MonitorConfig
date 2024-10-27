---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Set-MonitorBrightness

## SYNOPSIS
Sets the monitor brightness on the specified monitors.

## SYNTAX

### Default (Default)
```
Set-MonitorBrightness -Monitor <Monitor[]> [-Value] <UInt32> [<CommonParameters>]
```

### WMIOptions
```
Set-MonitorBrightness -Monitor <Monitor[]> [[-Value] <UInt32>] [-Timeout <UInt32>] [-RevertToPolicy]
 [-ALSEnabled <Boolean>] [-ALSBrightness <Byte>] [<CommonParameters>]
```

## DESCRIPTION
Sets the monitor brightness on the specified monitors.  
Also allows adjustment of various dynamic brightness settings for WMI displays.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor | Set-MonitorBrightness -Value 100
```

Sets the brightness value to 100 on all active monitors.

## PARAMETERS

### -ALSBrightness
Set the ambient light sensor brightness value.  
If brightness has been manually adjusted, the brightness policy has to be reverted by using the RevertToPolicy parameter.

```yaml
Type: Byte
Parameter Sets: WMIOptions
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ALSEnabled
Controls whether or not brightness should be adjusted by the Ambient Light Sensor.
If brightness has been manually adjusted, the brightness policy has to be reverted by using the RevertToPolicy parameter.

```yaml
Type: Boolean
Parameter Sets: WMIOptions
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Monitor
The monitor to adjust brightness settings for.

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

### -RevertToPolicy
Reverts the brightness policy to system managed if manual brightness values have previously been used.

```yaml
Type: SwitchParameter
Parameter Sets: WMIOptions
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Timeout
Sets the timeout for changing the brightness on WMIMonitors. It's rarely needed.

```yaml
Type: UInt32
Parameter Sets: WMIOptions
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
The brightness value to set.

```yaml
Type: UInt32
Parameter Sets: Default
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: UInt32
Parameter Sets: WMIOptions
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### MartinGC94.MonitorConfig.API.Monitor[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
