Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$ForceClean,
    [ValidateSet('All','Source','Docs')]
    [System.String]$BuildMode = 'All'
)

. .\buildutils.ps1
$buildInfo = Initialize-BuildEnvironment -FullInit

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
    if((Test-Path -PathType Container $buildInfo['BuildOutputPath']) -and $ForceClean )
    {
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $buildInfo['BuildOutputPath']
    }

    md $buildInfo['NuGetOutputPath'] -ErrorAction SilentlyContinue | Out-Null

    if($BuildSource)
    {
        .\Build-Source.ps1
    }

    if($BuildDocs)
    {
        .\Build-Docs.ps1
    }
}
finally
{
    popd
    $env:Path = $oldPath
}
