Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
)

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    ./Build-Interop.ps1 -AllowVsPreReleases:$AllowVsPreReleases -Configuration $Configuration

    ./Build-DotNet.ps1 -Configuration $Configuration
}
catch
{
    Write-Error "Build-Source.ps1 failed: $($_.Exception.Message)"
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}
