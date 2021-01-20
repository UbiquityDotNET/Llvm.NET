[CmdletBinding()]
param (
    [Parameter()]
    [String]
    $Configuration = "Release"
)

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    if ($env:BUILD_LLVM -eq "true") {
        ./Build-Llvm.ps1 -Configuration $Configuration
    }

    ./Move-LlvmBuild.ps1 -Configuration $Configuration

    ./Build-LibLlvm.ps1 -Configuration $Configuration
}
catch
{
    Write-Error "Build-Xplat.ps1 failed: $($_.Exception.Message)"
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}
