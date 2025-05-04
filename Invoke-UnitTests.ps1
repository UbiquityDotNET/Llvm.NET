<#
.SYNOPSIS
    Runs the unit tests as well as a validation run of the sample app

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this may be set to "Debug"

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.
#>
Param(
    [string]$Configuration="Release",
    [switch]$FullInit
)

try
{
    . .\repo-buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit

    Push-Location $BuildInfo["SrcRootPath"]
    try
    {
        Invoke-External dotnet test Ubiquity.NET.Llvm.slnx '-tl:off' '--logger:trx' '--no-build' '-s' '.\x64.runsettings'
    }
    finally
    {
        Pop-Location
    }

    # ensure the samples run - output not validated but they need to compile and run without crashing
    Write-Information 'Running sample apps for .NET Core'
    Push-Location (Join-path $buildInfo['BuildOutputPath'] 'bin\CodeGenWithDebugInfo\Release\net9.0')
    try
    {
        $testGenOutputPath = Join-Path $buildInfo['TestResultsPath'] 'M3'
        Write-Information "CodeGenWithDebugInfo M3 'Support Files\test.c' $testGenOutputPath"
        Invoke-External dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c' $testGenOutputPath

        $testGenOutputPath = Join-Path $buildInfo['TestResultsPath'] 'X64'
        Write-Information "CodeGenWithDebugInfo X64 'Support Files\test.c' $testGenOutputPath"
        Invoke-External dotnet CodeGenWithDebugInfo.dll X64 'Support Files\test.c' $testGenOutputPath

        Set-Location (Join-path $buildInfo['BuildOutputPath'] 'bin\OrcV2VeryLazy\Release\net9.0')
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

