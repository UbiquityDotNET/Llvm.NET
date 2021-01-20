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
        Remove-Item -Recurse -Force -Path $buildInfo['BuildOutputPath']
    }

    $includePath = (Join-Path $PSScriptRoot llvm include)
    $libPath = (Join-Path $PSScriptRoot llvm lib)
    if ((Test-Path -PathType Container $includePath) -and $ForceClean) {
        Write-Information "Cleaning headers folder from previous builds"
        Remove-Item -Recurse -Force -Path $includePath
    }
    if ((Test-Path -PathType Container $libPath) -and $ForceClean) {
        Write-Information "Cleaning libs folder from previous builds"
        Remove-Item -Recurse -Force -Path $libPath
    }

    New-Item -ItemType Directory $buildInfo['NuGetOutputPath'] -ErrorAction SilentlyContinue | Out-Null

    if($BuildSource)
    {
        ./Build-Xplat.ps1 -Configuration $Configuration
        ./Build-Source.ps1 -AllowVsPreReleases:$AllowVsPreReleases -Configuration $Configuration
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
