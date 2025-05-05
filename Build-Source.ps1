<#
.SYNOPSIS
    Builds just the source code to produce the binaries and NUGET packages for the Ubiquity.NET.Llvm libraries

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this may be set to "Debug"

.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.

.PARAMETER ZipNuget
    Zips all of the nuget packages into a single zip. This is NOT ordinarily needed and consumes a significant abount of time
    so it is provided as a flag. Publishing of NUGET packages is now part of the GitHub actions for automated builds and does
    not need this.
#>
Param(
    [string]$Configuration="Release",
    [switch]$FullInit
)

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    # Pull in the repo specific support and force a full initialization of all the environment
    # based on the switch parameter. Normally FullInit is done in Build-All, which calls this
    # script. But for a local "inner loop" development this might be the only script used.
    . .\repo-buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit

    # build the Managed code support
    Write-Information "dotnet build 'src\Ubiquity.NET.Llvm.slnx' -c $Configuration"
    Invoke-External dotnet build --tl:off 'src\Ubiquity.NET.Llvm.slnx' '-c' $Configuration
}
catch
{
    # Everything from the official docs to the various articles in the blog-sphere say this isn't needed
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
