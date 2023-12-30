---
external help file: MonitorConfig.dll-Help.xml
Module Name: MonitorConfig
online version:
schema: 2.0.0
---

# Set-MonitorColorSettings

## SYNOPSIS
Allows adjustment of various color related settings on the specified monitors.

## SYNTAX

```
Set-MonitorColorSettings -Monitor <VCPMonitor[]> [-RedDrive <UInt32>] [-RedGain <UInt32>]
 [-GreenDrive <UInt32>] [-GreenGain <UInt32>] [-BlueDrive <UInt32>] [-BlueGain <UInt32>]
 [-RedSaturation <UInt32>] [-YellowSaturation <UInt32>] [-GreenSaturation <UInt32>] [-CyanSaturation <UInt32>]
 [-BlueSaturation <UInt32>] [-MagentaSaturation <UInt32>] [-ColorSaturation <UInt32>]
 [-ColorTemperature <VCPMonitorColorTemperature>] [-Contrast <UInt32>] [-GrayScaleWhiteExpansion <UInt32>]
 [-GrayScaleBlackExpansion <UInt32>] [-Gamma <UInt32>] [<CommonParameters>]
```

## DESCRIPTION
Allows adjustment of various color related settings on the specified monitors.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Monitor -Primary | Set-MonitorColorSettings -ColorSaturation 40 -RedGain 95
```

Adjusts the overall color saturation and gain for the red color on the primary display.

## PARAMETERS

### -BlueDrive
Adjusts the "drive" AKA "black" level for the blue color.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BlueGain
Adjusts the "gain" AKA "tint" level for the blue color.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BlueSaturation
Adjusts how saturated the blue color should be.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ColorSaturation
Adjusts how saturated the overall image should be.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ColorTemperature
Changes the color temperature on the display.

```yaml
Type: VCPMonitorColorTemperature
Parameter Sets: (All)
Aliases:
Accepted values: _None, _4000k, _5000k, _6500k, _7500k, _8200k, _9300k, _10000k, _11500k

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Contrast
Adjusts the contrast level on the display.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CyanSaturation
Adjusts how saturated the cyan color should be.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Gamma
Adjusts the gamma level on the display.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GreenDrive
Adjusts the "drive" AKA "black" level for the green color.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GreenGain
Adjusts the "gain" AKA "tint" level for the green color.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GreenSaturation
Adjusts how saturated the green color should be.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MagentaSaturation
Adjusts how saturated the magenta color should be.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Monitor
The monitor(s) to get color info from.

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

### -RedDrive
Adjusts the "drive" AKA "black" level for the red color.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RedGain
Adjusts the "gain" AKA "tint" level for the red color.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RedSaturation
Adjusts how saturated the red color should be.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -YellowSaturation
Adjusts how saturated the yellow color should be.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GrayScaleBlackExpansion
Expands the gray scale in the near black region, making darks appear more light or gray.  
Values between 0-3 can be used.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GrayScaleWhiteExpansion
Expands the gray scale in the near white region, making whites appear more dark or gray.  
Values between 0-3 can be used.

```yaml
Type: UInt32
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

### System.Object
## NOTES

## RELATED LINKS
