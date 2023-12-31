@{
    RootModule             = "MonitorConfig.dll"
    ModuleVersion          = {0}
    CompatiblePSEditions   = @("Core", "Desktop")
    GUID                   = '552fa35c-0b32-42f6-858a-64d5ed0b05e6'
    Author                 = 'MartinGC94'
    CompanyName            = 'Unknown'
    Copyright              = '(c) 2023 MartinGC94. All rights reserved.'
    Description            = 'Manage brightness and other monitor settings with DDC/CI and WMI.'
    PowerShellVersion      = '5.1'
    FormatsToProcess       = @({1})
    FunctionsToExport      = @()
    CmdletsToExport        = @({2})
    VariablesToExport      = @()
    AliasesToExport        = @()
    DscResourcesToExport   = @()
    FileList               = @({3})
    PrivateData            = @{
        PSData = @{
             Tags         = @("Display", "Monitor", "Settings", "Options", "Configuration", "Config", "DDC", "DDC/CI", "MCCS", "Brightness", "Contrast")
             ProjectUri   = 'https://github.com/MartinGC94/MonitorConfig'
             ReleaseNotes = @'
{4}
'@
        }
    }
}