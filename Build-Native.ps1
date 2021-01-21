Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -AllowVsPreReleases:$AllowVsPreReleases

    .\Move-LlvmBuild.ps1

    $buildOutputDir = Join-Path src Interop LibLLVM BuildOutput
    if (Test-Path $buildOutputDir) {
        Write-Verbose "Cleaning out the old data from $buildOutputDir"
        Remove-Item -Path $buildOutputDir -Recurse -Force | Out-Null
    }
    New-Item -Path $buildOutputDir -ItemType Directory

    Write-Information "Building LibLLVM"
    pushd $buildOutputDir
    try
    {
        cmake (Resolve-Path ..)
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
        popd
    }

    New-Item -ErrorAction SilentlyContinue -ItemType Directory -Path (Join-Path $buildInfo['BuildOutputPath'] xplat win-x64)
    Copy-Item -Force -Path (Join-Path $buildOutputDir $Configuration *) (Join-Path $buildInfo['BuildOutputPath'] xplat win-x64)
}
finally
{
    popd
    $env:Path = $oldPath
}
