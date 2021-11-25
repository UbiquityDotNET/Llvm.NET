<#
.SYNOPSIS
    Runs the unit tests as well as a validation run of the sample app

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this may be set to "Debug"

.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.
#>
Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$FullInit
)

try
{
    . .\repo-buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit -AllowVsPreReleases:$AllowVsPreReleases

    $testsFailed = $false

    Write-Information 'Running Interop tests...'
    $testsFailed = $testsFailed -or (Invoke-DotNetTest $buildInfo 'src\Interop\InteropTests\InteropTests.csproj')

    Write-Information 'Running Core library tests...'
    $testsFailed = $testsFailed -or (Invoke-DotNetTest $buildInfo 'src\Ubiquity.NET.Llvm.Tests\Ubiquity.NET.Llvm.Tests.csproj')

    Write-Information 'Running JIT library tests...'
    $testsFailed = $testsFailed -or (Invoke-DotNetTest $buildInfo 'src\Ubiquity.NET.Llvm.JIT.Tests\Ubiquity.NET.Llvm.JIT.Tests.csproj')

    Write-Information 'Running tests for Kaleidoscope Samples...'
    $testsFailed = $testsFailed -or (Invoke-DotNetTest $buildInfo 'Samples\Kaleidoscope\Kaleidoscope.Tests\Kaleidoscope.Tests.csproj')

    Write-Information 'Running sample app for .NET Core'
    Push-Location (Join-path $buildInfo['BuildOutputPath'] 'bin\CodeGenWithDebugInfo\Release\net5.0')
    try
    {
        dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c' $buildInfo['TestResultsPath']
        $testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
    }
    finally
    {
        Pop-Location
    }

    if($testsFailed)
    {
        throw "One or more tests failed - Build should fail"
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

