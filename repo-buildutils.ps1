<#
.SYNOPSIS
    Project/Repo specific extensions to common support

.DESCRIPTION
    This provides repository specific functionality used by the various
    scripts. It will import the common repo neutral module such that this
    can be considered an extension of that module. It is expected that the
    various build scrips will "dot source" this one to consume common
    functionality.
#>

# reference the common build library. This library is intended
# for re-use across multiple repositories so should remain independent
# of the particular details of any specific repository. Over time, this
# may migrate to a git sub module for easier sharing between projects.
using module 'PSModules\CommonBuild\CommonBuild.psd1'

Set-StrictMode -version 3.0

$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

function Install-LlvmLibs($buildInfo)
{
<#
.SYNOPSIS
    Installs the LLVM libs, downloading them if not already present

.PARAMETER buildInfo
    Build information Hashtable containing the various build properties and directories.
    This should come from a call to Get-BuildInformation to ensure all the required
    directory path properties are set up correctly.
#>
    $destPath = $buildInfo['LlvmLibsRoot']
    $packageReleaseName = $buildInfo['LlvmLibsPackageReleaseName']
    if(!(Test-Path -PathType Container $destPath))
    {
        if(!(Test-Path -PathType Container $buildInfo['DownloadsPath']))
        {
            md $buildInfo['DownloadsPath'] | Out-Null
        }

        $localLlvmLibs7zPath = Join-Path $buildInfo['DownloadsPath'] "llvm-libs-$packageReleaseName.7z"
        if( !( test-path -PathType Leaf $localLlvmLibs7zPath ) )
        {
            $release = Get-GitHubTaggedRelease UbiquityDotNet 'Llvm.Libs' "v$packageReleaseName"
            if($release)
            {
                $asset = (Get-GitHubTaggedRelease UbiquityDotNet 'Llvm.Libs' "v$packageReleaseName").assets[0]
                Invoke-WebRequest -UseBasicParsing -Uri $asset.browser_download_url -OutFile $localLlvmLibs7zPath
            }
            else
            {
                throw "LLVM library package release 'v$packageReleaseName' not found!"
            }
        }

        Expand-7zArchive $localLlvmLibs7zPath $destPath
    }
}

function Initialize-BuildEnvironment
{
<#
.SYNOPSIS
    Initializes the build environment for the build scripts

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.

.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.DESCRIPTION
    This script is used to initialize the build environment in a central place, it returns the
    build info Hashtable with properties determined for the build. Script code should use these
    properties instead of any environment variables. While this script does setup some environment
    variables for non-script tools (i.e., MSBuild) script code should not rely on those.

    This script will setup the PATH environment variable to contain the path to MSBuild so it is
    readily available for all subsequent script code.

    Environment variables set for non-script tools:

    | Name               | Description |
    |--------------------|-------------|
    | IsAutomatedBuild   | "true" if in an automated build environment "false" for local developer builds |
    | IsPullRequestBuild | "true" if this is a build from an untrusted pull request (limited build, no publish etc...) |
    | IsReleaseBuild     | "true" if this is an official release build |
    | CiBuildName        | Name of the build for Constrained Semantic Version construction |
    | BuildTime          | ISO-8601 formatted time stamp for the build (local builds are based on current time, automated builds use the time from the HEAD commit)

    The Hashtable returned from this function includes all the values retrieved from
    the common build function Initialize-CommonBuildEnvironment plus additional repository specific
    values. In essence, the result is like a derived type from the common base. The
    additional properties added are:

    | Name                       | Description                                                                                            |
    |----------------------------|--------------------------------------------------------------------------------------------------------|
    | LlvmLibsRoot               | Root of the LLVM libraries (where they are extracted to after download)                                |
    | LlvmVersion                | LLVM version the build is targeting                                                                    |
    | LlvmLibsPackageReleaseName | Release name of the LLVM libs package to download                                                      |
#>
    # support common parameters
    [cmdletbinding()]
    Param([switch]$FullInit,
          [switch]$AllowVsPreReleases
         )

    # use common repo-neutral function to perform most of the initialization
    $buildInfo = Initialize-CommonBuildEnvironment -FullInit:$FullInit -AllowVsPreReleases:$AllowVsPreReleases
    $buildInfo['LlvmLibsRoot'] = Join-Path $PSScriptRoot 'llvm'
    $buildInfo['LlvmVersion'] = "10.0.0"
    $buildInfo['LlvmLibsPackageReleaseName'] = "$($buildInfo['LlvmVersion'])-msvc-16.5"

    if($FullInit)
    {
        Show-FullBuildInfo $buildInfo
    }

    return $buildInfo
}

