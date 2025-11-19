function Initialize-CommonBuildEnvironment
{
<#
.SYNOPSIS
    Initializes the build environment for the build scripts

.PARAMETER RepoRoot
    Root folder of the repository hosting this build

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.

.DESCRIPTION
    This script is used to initialize the build environment in a central place, it returns the
    build info Hashtable with properties determined for the build. Script code should use these
    properties instead of any environment variables. While this script does setup some environment
    variables for non-script tools (i.e., MSBuild) script code should not rely on those.

    Environment variables set for non-script tools:

    | Name               | Description |
    |--------------------|-------------|
    | IsAutomatedBuild   | "true" if in an automated build environment "false" for local developer builds |
    | IsPullRequestBuild | "true" if this is a build from an untrusted pull request (limited build, no publish etc...) |
    | IsReleaseBuild     | "true" if this is an official release build |
    | CiBuildName        | Name of the build for Constrained Semantic Version construction |
    | BuildTime          | ISO-8601 formatted time stamp for the build (local builds are based on current time, automated builds use the time from the HEAD commit)

    This function returns a Hashtable containing properties for the current build with the following properties:

    | Name                | Description                          |
    |---------------------|--------------------------------------|
    | RepoRootPath        | Root of the repository for the build |
    | BuildOutputPath     | Base directory for all build output during the build |
    | NuGetRepositoryPath | NuGet 'packages' directory for C++ projects using packages.config |
    | NuGetOutputPath     | Location where NuGet packages created during the build are placed |
    | SrcRootPath         | Root of the source code for this repository |
    | DocsOutputPath      | Root path for the generated documentation for the project |
    | BinLogsPath         | Path to where the binlogs are generated for PR builds to allow diagnosing failures in the automated builds |
    | TestResultsPath     | Path to where test results are placed. |
    | DownloadsPath       | Location where any downloaded files, used by the build are placed |
    | ToolsPath           | Location of any executable tools downloaded for the build (Typically expanded from a compressed download) |
    | CurrentBuildKind    | Results of a call to Get-CurrentBuildKind |
    | VersionTag          | Git tag name for this build if released |
#>
    # support common parameters
    [cmdletbinding()]
    [OutputType([hashtable])]
    Param(
        [ValidateNotNullorEmpty()]
        [string]$repoRoot,
        [switch]$FullInit
    )

    try
    {
        # Script code should ALWAYS use the global CurrentBuildKind
        $currentBuildKind = Get-CurrentBuildKind

        # set/reset legacy environment vars for non-script tools (i.e. msbuild.exe)
        $env:IsAutomatedBuild = $currentBuildKind -ne [BuildKind]::LocalBuild
        $env:IsPullRequestBuild = $currentBuildKind -eq [BuildKind]::PullRequestBuild
        $env:IsReleaseBuild = $currentBuildKind -eq [BuildKind]::ReleaseBuild

        switch($currentBuildKind)
        {
            ([BuildKind]::LocalBuild) { $env:CiBuildName = 'ZZZ' }
            ([BuildKind]::PullRequestBuild) { $env:CiBuildName = 'PRQ' }
            ([BuildKind]::CiBuild) { $env:CiBuildName = 'BLD' }
            ([BuildKind]::ReleaseBuild) { $env:CiBuildName = '' }
            default { throw "Invalid build kind" }
        }

        # get the ISO-8601 formatted time stamp of the HEAD commit or the current UTC time for local builds
        # for FullInit force a re-capture of the time stamp.
        if(!$env:BuildTime -or $FullInit)
        {
            # for any automated builds use the ISO-8601 time stamp of the current commit
            if($currentBuildKind -ne [BuildKind]::LocalBuild)
            {
                $env:BuildTime = (git show -s --format=%cI)
            }
            else
            {
                $env:BuildTime = ([System.DateTime]::UtcNow.ToString("o"))
            }
        }

        # On Windows setup the equivalent of a Developer prompt.
        #
        # Other platform runners may have different defaulted paths etc...
        # to account for here.
        if ($IsWindows)
        {
            Write-Verbose "Configuring VS tools support"

            # For windows force a common VS tools prompt;
            # Sadly most of this is undocumented and found from various sites
            # spelunking the process. This isn't as bad as it might seem as the
            # installer will create persistent use of this as a Windows Terminal
            # "profile" and the actual command is exposed.
            if($null -eq (Find-OnPath vswhere))
            {
                # NOTE: automated builds in Github do NOT include winget (for reasons unknown)
                # However, they do contain VSWHERE so should not hit this.
                winget install Microsoft.VisualStudio.Locator | Out-Null

                # location is fixed and the same no matter what version!
                $env:PATH +=';%ProgramFiles(x86)%\Microsoft Visual Studio\Installer'
            }

            $vsShellModulePath = vswhere -latest -find **\Microsoft.VisualStudio.DevShell.dll
            $vsToolsArch = Get-VsArchitecture
            if(!$vsShellModulePath)
            {
                throw "VS shell module not found!"
            }

            Import-Module $vsShellModulePath | Out-Null
            $vsInstanceId = vswhere -latest -format value -property InstanceId
            Enter-VsDevShell $vsInstanceId -SkipAutomaticLocation -DevCmdArguments "-arch=$vsToolsArch -host_arch=$vsToolsArch" | Out-Null
        }

        #Start with standard build paths then add additional values to the hashtable
        $buildInfo = Get-DefaultBuildPaths $repoRoot
        $buildInfo['CurrentBuildKind'] = $currentBuildKind
        $buildInfo['VersionTag'] = Get-BuildVersionTag $buildInfo
        return $buildInfo
    }
    catch
    {
        # everything from the official docs to the various articles in the blog-sphere says this isn't needed
        # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
        # information is retained and the error reported will include the correct source file and line number
        # data for the error. Without this, only the error message is retained and the location information is
        # Line 1, Column 1, of the outer most script file, or the calling location neither of which is useful.
        throw
    }
}
