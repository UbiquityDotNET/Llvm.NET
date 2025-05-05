# This script is used for debugging the module when using the
# [Powershell Tools for VisualStudio 2022](https://marketplace.visualstudio.com/items?itemName=AdamRDriscoll.PowerShellToolsVS2022)
# It is MOST useful for computing and reporting the set of functions to export from this module for the PSD1 file.
Import-Module $PSScriptRoot\CommonBuild.psd1 -Force -Verbose
CommonBuild\Get-FunctionsToExport

# This is private and should not be exported - Should generate an error
$buildInfo = @{ BuildOutputPath = 'foo/bar' }
$config = New-CMakeConfig "x64-Release" "release" $buildInfo 'foo/bar/cmake_src'
if(!$config -or $config -isnot [hashtable])
{
    throw "Null or wrong type returned!"
}

$testArgs = @('a', 'b', 'c')
Invoke-External where.exe $testArgs
