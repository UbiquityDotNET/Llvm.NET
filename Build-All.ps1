Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$ForceClean,
    [ValidateSet('All','Source','Docs')]
    [System.String]$BuildMode = 'All'
)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit

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

    if($BuildSource)
    {
        .\Build-Source.ps1
    }

    if($BuildDocs)
    {
        .\Build-Docs.ps1
    }
}
catch
{
    Write-Error $_.Exception.Message
}
finally
{
    popd
    $env:Path = $oldPath
}

Write-Information "Done build"
