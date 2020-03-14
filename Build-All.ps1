Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$ForceClean,
    [ValidateSet('All','Source','Docs')]
    [System.String]$BuildMode = 'All'
)
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

. .\buildutils.ps1
Initialize-BuildEnvironment -FullInit

pushd $PSScriptRoot
$oldPath = $env:Path
$BuildSource = $false
$BuildDocs = $false;

switch($BuildMode)
{
'All' { $BuildSource = $true; $BuildDocs = $true; }
'Source' { $BuildSource = $true }
'Docs' { $BuildDocs = $true }
}

try
{
    if( (Test-Path -PathType Container $BuildPaths.BuildOutputPath) -and $ForceClean )
    {
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $BuildPaths.BuildOutputPath
    }

    md $BuildPaths.NuGetOutputPath -ErrorAction SilentlyContinue | Out-Null

    if($BuildSource)
    {
        .\Build-Source.ps1 -BuildInfo $BuildInfo
    }

    if($BuildDocs)
    {
        .\Build-Docs.ps1 -BuildInfo $BuildInfo
    }
}
finally
{
    popd
    $env:Path = $oldPath
}
