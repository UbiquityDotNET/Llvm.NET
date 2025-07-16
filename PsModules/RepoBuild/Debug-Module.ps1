# This script is used for debugging the module when using the
# [PowerShell Tools for VisualStudio 2022](https://marketplace.visualstudio.com/items?itemName=AdamRDriscoll.PowerShellToolsVS2022)
Import-Module $PSScriptRoot\RepoBuild.psd1 -Force -Verbose

# Get and show the Functions to export to allow easy updates of the PSD file
# this eliminates most of the error prone tedious nature of manual updates.
RepoBuild\Get-FunctionsToExport

