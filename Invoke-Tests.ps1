using module "PSModules/CommonBuild/CommonBuild.psd1"
using module "PSModules/RepoBuild/RepoBuild.psd1"

<#
.SYNOPSIS
    Script to invoke tests of all the code in this repo

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this
    may be set to "Debug".

.DESCRIPTION
    This script is used by the automated build to run tests for the actual build. The Ubiquity.NET
    family of projects all employ a PowerShell driven build that is generally divorced from the
    automated build infrastructure used. This is done for several reasons, but the most
    important ones are the ability to reproduce the build locally for inner development and
    for flexibility in selecting the actual back end. The back ends have changed a few times
    over the years and re-writing the entire build in terms of those back ends each time is
    a lot of wasted effort. Thus, the projects settled on PowerShell as the core automated
    build tooling.
#>
[cmdletbinding()]
Param(
    [string]$Configuration="Release"
)

Set-StrictMode -Version 3.0

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    # Pull in the repo specific support but don't force a full initialization of all the environment
    # as this assumes a build is already complete. This does NOT restore or build ANYTHING. It just
    # runs the tests.
    $buildInfo = Initialize-BuildEnvironment

    Push-Location $BuildInfo['SrcRootPath']
    try
    {
        Invoke-External dotnet test Ubiquity.NET.Llvm.slnx '-c' $Configuration '-tl:off' '--logger:trx' '--no-build' '-s' '.\x64.runsettings'
    }
    finally
    {
        Pop-Location
    }

    # ensure the samples run - output not validated but they need to compile and run without crashing
    Write-Information 'Running sample apps for .NET Core'
    $sampleTFM = "net8.0"
    Push-Location (Join-path $buildInfo['BuildOutputPath'] 'bin' 'CodeGenWithDebugInfo' 'Release' $sampleTFM)
    try
    {
        $testGenOutputPath = Join-Path $buildInfo['TestResultsPath'] 'M3'
        $testFileRelativePath = Join-Path 'Support Files' 'test.c'
        Write-Information "CodeGenWithDebugInfo M3 '$testFileRelativePath' $testGenOutputPath"
        Invoke-External dotnet CodeGenWithDebugInfo.dll M3 $testFileRelativePath $testGenOutputPath

        $testGenOutputPath = Join-Path $buildInfo['TestResultsPath'] 'X64'
        Write-Information "CodeGenWithDebugInfo X64 '$testFileRelativePath' $testGenOutputPath"
        Invoke-External dotnet CodeGenWithDebugInfo.dll X64 $testFileRelativePath $testGenOutputPath

        Set-Location (Join-path $buildInfo['BuildOutputPath'] 'bin' 'OrcV2VeryLazy' 'Release' $sampleTFM)
        Invoke-External dotnet OrcV2VeryLazy.dll
    }
    finally
    {
        Pop-Location
    }
}
catch
{
    # everything from the official docs to the various articles in the blog-sphere says this isn't needed
    # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
    # information is retained and the error reported will include the correct source file and line number
    # data for the error. Without this, only the error message is retained and the location information is
    # Line 1, Column 1, of the outer most script file, which is, of course, completely useless.
    throw
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}

Write-Information "Done tests"
