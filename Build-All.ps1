Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClean,
    [ValidateSet('All','Source','Docs')]
    [System.String]$BuildMode = 'All'
)

. .\buildutils.ps1
Initialize-BuildEnvironment -Verbose

pushd $PSScriptRoot
$oldPath = $env:Path
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"
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
    $buildPaths = Get-BuildPaths $PSScriptRoot

    Write-Information "Build Paths:"
    Write-Information ($buildPaths | Format-Table | Out-String)

    if( (Test-Path -PathType Container $buildPaths.BuildOutputPath) -and !$NoClean )
    {
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $buildPaths.BuildOutputPath
    }

    md $buildPaths.NuGetOutputPath -ErrorAction SilentlyContinue| Out-Null

    $BuildInfo = Get-BuildInformation $buildPaths

    if($BuildSource)
    {
        .\Build-Source.ps1 -BuildInfo $BuildInfo
    }

    if($BuildDocs)
    {
        .\Build-Docs.ps1 -BuildInfo $BuildInfo
    }

    # AppVeyor specific artifact push. (Should be part of YML so scripts are build infra neutral...)
    if( $env:APPVEYOR_PULL_REQUEST_NUMBER )
    {
        Get-ChildItem  -Filter *.binlog $buildPaths.BinLogsPath | %{ Push-AppveyorArtifact $_.FullName }
    }
}
finally
{
    popd
    $env:Path = $oldPath
}
