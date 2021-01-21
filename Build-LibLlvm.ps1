Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    # Treat Release build config as RelWithDebInfo so that pdb is produced.
    if ($Configuration -eq "Release") 
    {
        $Configuration = "RelWithDebInfo"
    }

    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -AllowVsPreReleases:$AllowVsPreReleases

    $srcPath = Join-Path src Interop LibLLVM | Resolve-Path
    $buildOutputDir = Join-Path $buildInfo['BuildOutputPath'] LibLlvm
    if (Test-Path $buildOutputDir) {
        Write-Verbose "Cleaning out the old data from $buildOutputDir"
        Remove-Item -Path $buildOutputDir -Recurse -Force | Out-Null
    }
    New-Item -Path $buildOutputDir -ItemType Directory

    Write-Information "Building LibLLVM"
    Push-Location $buildOutputDir
    try
    {
        cmake $srcPath
        if($LASTEXITCODE -ne 0 )
        {
            throw "LibLLVM CMake generate exited with code: $LASTEXITCODE"
        }
        cmake --build . --target "ALL_BUILD" --config $Configuration
        if($LASTEXITCODE -ne 0 )
        {
            throw "LibLLVM CMake build exited with code: $LASTEXITCODE"
        }
    }
    finally
    {
        Pop-Location
    }

    New-Item -ErrorAction SilentlyContinue -ItemType Directory -Path (Join-Path $buildInfo['BuildOutputPath'] xplat win-x64)
    Copy-Item -Force -Path (Join-Path $buildOutputDir $Configuration *) (Join-Path $buildInfo['BuildOutputPath'] xplat win-x64)
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}
