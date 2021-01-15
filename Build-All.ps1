Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$ForceClean,
    [ValidateSet('All','Source','Docs')]
    [System.String]$BuildMode = 'All'
)

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    . ./buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit -AllowVsPreReleases:$AllowVsPreReleases

    $BuildSource = $false
    $BuildDocs = $false;

    switch($BuildMode)
    {
    'All' { $BuildSource = $true; $BuildDocs = $true; }
    'Source' { $BuildSource = $true }
    'Docs' { $BuildDocs = $true }
    }

    if((Test-Path -PathType Container $buildInfo['BuildOutputPath']) -and $ForceClean )
    {
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $buildInfo['BuildOutputPath']
    }

    md $buildInfo['NuGetOutputPath'] -ErrorAction SilentlyContinue | Out-Null

    $env:BUILD_CONFIG = $Configuration

    if($BuildSource)
    {
        ./Build-Xplat.ps1
        ./Build-Source.ps1 -AllowVsPreReleases:$AllowVsPreReleases
    }

    if($BuildDocs)
    {
        ./Build-Docs.ps1 -AllowVsPreReleases:$AllowVsPreReleases
    }
}
catch
{
    Write-Error $_.Exception.Message
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}

Write-Information "Done build"
