<#
.SYNOPSIS
    Script to build all of the LLvm.NET code base

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this may be set to "Debug"

.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.PARAMETER ForceClean
    Forces a complete clean (Recursive delete of the build output)

.PARAMETER BuildMode
    Specifies the build mode, which may be one of the following:

    |Name    | Description                                                               |
    |--------|---------------------------------------------------------------------------|
    | Source | Builds only the source code. This is useful for local development.        |
    | Docs   | Builds only the docs. Saves on build time when only updating docs topics. |
    | All    | [Default] Builds everything                                               |

.DESCRIPTION
    This script is used by the automated build to perform the actual build. The Ubiquity.NET
    family of projects all employ a PowerShell driven build that is generally divorced from
    the automated build infrastructure used. This is done for several reasons, but the most
    important ones are the ability to reproduce the build locally for inner development and
    for flexibility in selecting the actual back end. The back ends have changed a few times
    over the years and re-writing the entire build in terms of those back ends each time is
    a lot of wasted effort. Thus, the projects settled on PowerShell as the core automated
    build tooling.
#>
[cmdletbinding()]
Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$ForceClean,
    [ValidateSet('All','Source','Docs')]
    [System.String]$BuildMode = 'All'
)

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    # Pull in the repo specific support and force a full initialization of all the environment
    # as this is a top level build command.
    . .\repo-buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit -AllowVsPreReleases:$AllowVsPreReleases
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
        remove-Item -Recurse -Force -Path $buildInfo['BuildOutputPath'] -ProgressAction SilentlyContinue
    }

    New-Item -ItemType Directory $buildInfo['NuGetOutputPath'] -ErrorAction SilentlyContinue | Out-Null

    if($BuildSource)
    {
        .\Build-Source.ps1 -AllowVsPreReleases:$AllowVsPreReleases
    }

    if($BuildDocs)
    {
        .\Build-Docs.ps1 -AllowVsPreReleases:$AllowVsPreReleases
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

Write-Information "Done build"
