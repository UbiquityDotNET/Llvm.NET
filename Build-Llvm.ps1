# Helpers for cmake builds
using module 'PsModules\CommonBuild\CommonBuild.psd1'

Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
)

. (Join-Path $PSScriptRoot CMake-Helpers.ps1)

function New-LlvmCMakeConfig(
    [string]$platform,
    [string]$config,
    [string]$baseBuild = (Join-Path (Get-Location) BuildOutput),
    [string]$srcRoot = (Get-Location)
    )
{
    [CMakeConfig]$cmakeConfig = New-Object CMakeConfig -ArgumentList $platform, $config, $baseBuild, $srcRoot
    $cmakeConfig.CMakeBuildVariables = @{
        LLVM_ENABLE_RTTI = "ON"
        LLVM_BUILD_TOOLS = "OFF"
        LLVM_BUILD_UTILS = "OFF"
        LLVM_BUILD_DOCS = "OFF"
        LLVM_BUILD_RUNTIME = "OFF"
        LLVM_BUILD_RUNTIMES = "OFF"
        LLVM_OPTIMIZED_TABLEGEN = "ON"
        LLVM_REVERSE_ITERATION = "ON"
        LLVM_TARGETS_TO_BUILD  = "all"
        LLVM_INCLUDE_DOCS = "OFF"
        LLVM_INCLUDE_EXAMPLES = "OFF"
        LLVM_INCLUDE_GO_TESTS = "OFF"
        LLVM_INCLUDE_RUNTIMES = "OFF"
        LLVM_INCLUDE_TESTS = "OFF"
        LLVM_INCLUDE_TOOLS = "OFF"
        LLVM_INCLUDE_UTILS = "OFF"
        LLVM_ADD_NATIVE_VISUALIZERS_TO_SOLUTION = "ON"
    }
    return $cmakeConfig
}

function Invoke-Build()
{
    # Verify CMake version info
    Assert-CMakeInfo ([Version]::new(3, 12, 1))

    try
    {
        $timer = [System.Diagnostics.Stopwatch]::StartNew()
        Write-Information "Creating CMAKE configuration"
        $cmakeConfig = New-LlvmCMakeConfig "x64" $Configuration

        Write-Information "Generating CMAKE configuration $($cmakeConfig.Name)"
        Invoke-CMakeGenerate $cmakeConfig

        Write-Information "Building CMAKE configuration $($cmakeConfig.Name)"
        Invoke-CMakeBuild $cmakeConfig
    }
    finally
    {
        $timer.Stop()
        Write-Information "Build Finished - Elapsed Time: $($timer.Elapsed.ToString())"
    }
}

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1

    $plat = Get-Platform
    if ($plat -eq [Platform]::Windows) {
        .\Repair-WinBuild.ps1
    }

    cd llvm-project\llvm
    Invoke-Build 
}
catch
{
    Write-Host "##vso[task.logissue type=error;]$($_.Exception.Message)"
    Write-Error $_.Exception.Message
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}

Write-Information "Done building LLVM"
