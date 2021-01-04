Param(
    [string]$Configuration="Release"
)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment

    # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
    # and PackageReference isn't supported for native projects... [Sigh...]
    Write-Information "Restoring LibLLVM"
    Invoke-NuGet restore (Join-Path src Interop LibLLVM LibLLVM.vcxproj)

    $buildOutputDir = Join-Path src Interop LibLLVM BuildOutput
    if (Test-Path $buildOutputDir) {
        Write-Verbose "Cleaning out the old data from $buildOutputDir"
        Remove-Item -Path $buildOutputDir -Recurse -Force | Out-Null
    }
    New-Item -Path $buildOutputDir -ItemType Container

    $target = "all"
    if ($buildInfo['Platform'] -eq [platform]::Windows) {
        $target = "ALL_BUILD"
    }
    Write-Information "Building LibLLVM"
    cd $buildOutputDir
    cmake (Resolve-Path ..)
    if($LASTEXITCODE -ne 0 )
    {
        throw "LibLLVM CMake generate exited with code: $LASTEXITCODE"
    }
    cmake --build . --target $target --config $Configuration
    if($LASTEXITCODE -ne 0 )
    {
        throw "LibLLVM CMake build exited with code: $LASTEXITCODE"
    }
    cd $PSScriptRoot

    if ($buildInfo['Platform'] -eq [platform]::Windows) {
        New-Item -ErrorAction SilentlyContinue -ItemType Container -Path (Join-Path $buildInfo["NativeXplat"] win-x64)
        Copy-Item -Force -Path (Join-Path $buildOutputDir $Configuration *) (Join-Path $buildInfo["NativeXplat"] win-x64)
    } elseif ($buildInfo['Platform'] -eq [platform]::Linux) {
        New-Item -ErrorAction SilentlyContinue -ItemType Container -Path (Join-Path $buildInfo["NativeXplat"] linux-x64)
        ls $buildOutputDir
        Copy-Item -Force -Path (Join-Path $buildOutputDir libUbiquity.NET.LibLlvm.so) (Join-Path $buildInfo["NativeXplat"] linux-x64 Ubiquity.NET.LibLlvm.dll)
    } else {
        New-Item -ErrorAction SilentlyContinue -ItemType Container -Path (Join-Path $buildInfo["NativeXplat"] osx-x64)
        ls $buildOutputDir
        Copy-Item -Force -Path (Join-Path $buildOutputDir libUbiquity.NET.LibLlvm.dylib) (Join-Path $buildInfo["NativeXplat"] osx-x64 Ubiquity.NET.LibLlvm.dll)
    }
}
finally
{
    popd
    $env:Path = $oldPath
}
