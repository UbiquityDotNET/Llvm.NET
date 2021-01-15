Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    . ./buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -AllowVsPreReleases:$AllowVsPreReleases

    if ($env:OUTPUT_LLVM -eq "true") {
        Write-Host '##vso[task.logissue type=warning;]Exiting early after building LLVM'
        return
    }

    ./Build-Interop.ps1 -Configuration $env:BUILD_CONFIG -AllowVsPreReleases:$AllowVsPreReleases

    ./Build-DotNet.ps1 -Configuration $env:BUILD_CONFIG
}
catch
{
    Write-Host "##vso[task.logissue type=error;]Build-Source.ps1 failed: $($_.Exception.Message)"
    Write-Error "Build-Source.ps1 failed: $($_.Exception.Message)"
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}
