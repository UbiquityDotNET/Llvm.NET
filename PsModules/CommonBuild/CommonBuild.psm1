# Repository neutral common build support utilities
# This library is intended for use across multiple repositories
# and therefore, should only contain functionality that is independent
# of the particulars of any given repository.

function Ensure-PathExists
{
    param([Parameter(Mandatory=$true, ValueFromPipeLine)]$path)
    mkdir -Force -ErrorAction SilentlyContinue $path | Out-Null
}

function Get-DefaultBuildPaths([string]$repoRoot)
{
<#
.SYNOPSIS
    Gets the default set of paths for a build

.DESCRIPTION
    This function initializes a hash table with the default paths for a build. This
    allows for standardization of build output locations etc... across builds and repositories
    in the organization. The values set are as follows:

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
#>
    $buildOutputPath = ConvertTo-NormalizedPath (Join-Path $repoRoot 'BuildOutput')
    $buildPaths = @{
        RepoRootPath = $repoRoot
        BuildOutputPath = $buildOutputPath
        NuGetRepositoryPath = Join-Path $buildOutputPath 'packages'
        NuGetOutputPath = Join-Path $buildOutputPath 'NuGet'
        SrcRootPath = Join-Path $repoRoot 'src'
        DocsRepoPath = Join-Path $buildOutputPath 'docs'
        DocsOutputPath = Join-Path $buildOutputPath 'docs'
        BinLogsPath = Join-Path $buildOutputPath 'BinLogs'
        TestResultsPath = Join-Path $buildOutputPath 'Test-Results'
        DownloadsPath = Join-Path $repoRoot 'Downloads'
        ToolsPath = Join-Path $repoRoot 'Tools'
    }

    $buildPaths.GetEnumerator() | %{ Ensure-PathExists $_.Value }
    return $buildPaths
}

function Update-Submodules
{
<#
.SYNOPSIS
    Updates Git submodules for this repository
#>
    Write-Information "Updating submodules"
    git submodule -q update --init --recursive
}

function Find-OnPath
{
<#
.SYNOPSIS
    Searches for an executable on the current environment search path

.PARAMETER exeName
    The executable to search for

.NOTES
    This is a simple wrapper around the command line 'where' utility to select the first location found
#>
    [CmdletBinding()]
    Param( [Parameter(Mandatory=$True,Position=0)][string]$exeName)
    $path = $null
    Write-Information "Searching for $exeName..."
    try
    {
        $path = where.exe $exeName 2>$null | select -First 1
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

    if($path)
    {
        Write-Information "Found $exeName at: '$path'"
    }
    return $path
}

function ConvertTo-NormalizedPath([string]$path )
{
<#
.SYNOPSIS
    Converts a potentially relative folder path to an absolute one with a trailing delimiter

.PARAMETER path
    Path to convert

.NOTES
    The delimiters in the path are converted to the native system preferred form during conversion
#>
    if(![System.IO.Path]::IsPathRooted($path))
    {
        $path = [System.IO.Path]::Combine((pwd).Path,$path)
    }

    $path = [System.IO.Path]::GetFullPath($path)
    if( !$path.EndsWith([System.IO.Path]::DirectorySeparatorChar) -and !$path.EndsWith([System.IO.Path]::AltDirectorySeparatorChar))
    {
        $path = $path + [System.IO.Path]::DirectorySeparatorChar
    }
    return $path
}

function ConvertTo-PropertyList([hashtable]$hashTable)
{
<#
.SYNOPSIS
    Converts a hash table into a semi-colon delimited property list
#>
    return ( $hashTable.GetEnumerator() | %{ @{$true=$_.Key;$false= $_.Key + "=" + $_.Value }[[string]::IsNullOrEmpty($_.Value) ] } ) -join ';'
}

function Invoke-TimedBlock([string]$activity, [ScriptBlock]$block)
{
<#
.SYNOPSIS
    Invokes a script block with a timer

.PARAMETER activity
    Name of the activity to output as part of Write-Information messages for the timer

.PARAMETER block
    Script block to execute with the timer

.DESCRIPTION
    This will print a start (via Write-Information), start the timer, run the script block stop the timer
    then print a finish message indicating the total time the script block took to run.
#>
    $timer = [System.Diagnostics.Stopwatch]::StartNew()
    Write-Information "Starting: $activity"
    try
    {
        $block.Invoke()
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
        $timer.Stop()
        Write-Information "Finished: $activity - Time: $($timer.Elapsed.ToString())"
    }
}

function Expand-ArchiveStream([Parameter(Mandatory=$true, ValueFromPipeLine)]$src, [Parameter(Mandatory=$true)]$OutputPath)
{
<#
.SYNOPSIS
    Expands an archive stream

.PARAMETER src
    Input stream containing compressed ZIP archive data to expand

.PARAMETER OutputPath
    Out put destination for the decompressed data
#>
    $zipArchive = [System.IO.Compression.ZipArchive]::new($src)
    [System.IO.Compression.ZipFileExtensions]::ExtractToDirectory( $zipArchive, $OutputPath)
}

function Expand-StreamFromUri([Parameter(Mandatory=$true, ValueFromPipeLine)]$uri, [Parameter(Mandatory=$true)]$OutputPath)
{
<#
.SYNOPSIS
    Downloads and expands a ZIP file to the specified destination

.PARAMETER uri
    URI of the ZIP file to download and expand

.PARAMETER OutputPath
    Output folder to expand the ZIP contents into
#>
    $strm = (Invoke-WebRequest -UseBasicParsing -Uri $uri).RawContentStream
    Expand-ArchiveStream $strm $OutputPath
}

function Invoke-NuGet
{
<#
.SYNOPSIS
    Invokes NuGet with any arguments provided

.DESCRIPTION
    This will attempt to find Nuget.exe on the current path, and if not found will download the latest
    version from NuGet.org before running the command.
#>
    $buildPaths = Get-DefaultBuildPaths ([IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..')))
    $NuGetPaths = Find-OnPath NuGet.exe -ErrorAction Continue
    if( !$NuGetPaths )
    {
        $nugetToolsPath = Join-Path $buildPaths['ToolsPath'] 'NuGet.exe'
        if( !(Test-Path $nugetToolsPath))
        {
            # Download it from official NuGet release location
            Write-Verbose "Downloading Nuget.exe to $nugetToolsPath"
            Invoke-WebRequest -UseBasicParsing -Uri https://dist.NuGet.org/win-x86-commandline/latest/NuGet.exe -OutFile $nugetToolsPath
        }

        $env:Path = "$env:Path;$($buildPaths['ToolsPath'])"
    }

    Write-Information "NuGet $args"
    NuGet.exe $args
    $err = $LASTEXITCODE
    if($err -ne 0)
    {
        throw "Error running NuGet: $err"
    }
}

function Find-VSInstance([switch]$PreRelease, $Version = '[15.0, 18.0)', [string[]]$requiredComponents = @('Microsoft.Component.MSBuild'))
{
<#
.SYNOPSIS
    Finds an installed VS instance

.PARAMETER PreRelease
    Indicates if the search should include pre-release versions of Visual Studio

.PARAMETER version
    The version range to search for. [Default is '[15.0, 18.0)']

.PARAMETER requiredComponents
    The set of required components to search for. [Default is an empty array]

.DESCRIPTION
    Uses the official MS provided PowerShell module to find a VS instance. If the VSSetup
    module is not loaded it is loaded first. If it isn't installed, then the module is installed.
#>
    $forceModuleInstall = [System.Convert]::ToBoolean($env:IsAutomatedBuild)
    $existingModule = Get-InstalledModule -ErrorAction SilentlyContinue VSSetup
    if(!$existingModule)
    {
        Write-Information "Installing VSSetup module"
        Install-Module VSSetup -Scope CurrentUser -Force:$forceModuleInstall | Out-Null
    }

    $vs = Get-VSSetupInstance -Prerelease:$PreRelease |
          Select-VSSetupInstance -Version $Version -Require $requiredComponents |
          select -Last 1

    return $vs
}

function Find-MSBuild([switch]$AllowVsPreReleases)
{
<#
.SYNOPSIS
    Locates MSBuild if not already in the environment path

.DESCRIPTION
    Attempts to find MSBuild on the current environment path, if not found uses Find-VSInstance
    to locate a Visual Studio instance that can provide an MSBuild.

.PARAMETER AllowVsPreReleases
    Switch to indicate if the search for a VS Instance should include pre-release versions.
#>
    $foundOnPath = $true
    $msBuildPath = Find-OnPath msbuild.exe -ErrorAction Continue
    if( !$msBuildPath )
    {
        Write-Verbose "MSBuild not found on PATH attempting to locate VS installation"
        $vsInstall = Find-VSInstance -Prerelease:$AllowVsPreReleases
        if( !$vsInstall )
        {
            throw "MSBuild not found on PATH and No instances of VS found to use"
        }

        Write-Verbose "VS installation found: $($vsInstall | Format-List | Out-String)"
        $msBuildPath = [System.IO.Path]::Combine( $vsInstall.InstallationPath, 'MSBuild', '15.0', 'bin', 'MSBuild.exe')
        if(!(Test-Path -PathType Leaf $msBuildPath))
        {
            $msBuildPath = [System.IO.Path]::Combine( $vsInstall.InstallationPath, 'MSBuild', 'current', 'bin', 'MSBuild.exe')
        }

        $foundOnPath = $false
    }

    if( !(Test-Path -PathType Leaf $msBuildPath ) )
    {
        Write-Information 'MSBuild not found'
        return $null
    }

    Write-Verbose "MSBuild Found at: $msBuildPath"
    $versionInfo = & $msBuildPath -version
    return @{ FullPath=$msBuildPath
              BinPath=[System.IO.Path]::GetDirectoryName( $msBuildPath )
              FoundOnPath=$foundOnPath
              Version = $versionInfo[-1]
            }
}

function Invoke-MSBuild([string]$project, [hashtable]$properties, $targets, $loggerArgs=@(), $additionalArgs=@())
{
<#
.SYNOPSIS
    Invokes MSBuild with the options provided

.PARAMETER project
    The project to build

.PARAMETER properties
    Hash table of properties to provide to MSBuild

.PARAMETER targets
    MSBuild targets to build

.PARAMETER loggerArgs
    MSBuild logger arguments

.PARAMMETER additionalArgs
    Additional args for MSBuild
#>
    $projName = [System.IO.Path]::GetFileNameWithoutExtension($project)
    $msbuildArgs = @($project, "/m", "/t:$($targets -join ';')") + $loggerArgs + $additionalArgs
    if( $properties )
    {
        $msbuildArgs += @( "/p:$(ConvertTo-PropertyList $properties)" )
    }

    Write-Information "msbuild $($msbuildArgs -join ' ')"
    msbuild $msbuildArgs
    if($LASTEXITCODE -ne 0)
    {
        Write-Error "Error running msbuild: $LASTEXITCODE"  -ErrorAction Stop
    }
}

function Find-7Zip()
{
<#
.SYNOPSIS
    Attempts to find 7zip console command

.DESCRIPTION
    This will try to find 7z.exe on the current path, and if not found tries to find the registered install location
#>
    $path7Z = Find-OnPath '7z.exe' -ErrorAction SilentlyContinue
    if(!$path7Z)
    {
        if( Test-Path -PathType Container HKLM:\SOFTWARE\7-Zip )
        {
            $path7Z = Join-Path (Get-ItemProperty HKLM:\SOFTWARE\7-Zip\ 'Path').Path '7z.exe'
        }

        if( !$path7Z -and ($env:PROCESSOR_ARCHITEW6432 -eq "AMD64") )
        {
            $hklm = [Microsoft.Win32.RegistryKey]::OpenBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine, [Microsoft.Win32.RegistryView]::Registry64)
            $subKey = $hklm.OpenSubKey("SOFTWARE\7-Zip")
            $root = $subKey.GetValue("Path")
            if($root)
            {
                $path7Z = Join-Path $root '7z.exe'
            }
        }
    }

    if(!$path7Z -or !(Test-Path -PathType Leaf $path7Z ) )
    {
        throw "Can't find 7-zip command line executable"
    }

    return $path7Z
}

Function Expand-7zArchive([string]$Path, [string]$Destination)
{
<#
.SYNOPSIS
    Expands an archive packed with 7-Zip
#>
    $7zPath = Find-7Zip
    $7zArgs = @(
        'x'  # eXtract with full path
        '-y' # auto apply Yes for all prompts
        "`"-o$($Destination)`""  # Output directory
        "`"$($Path)`"" # 7z file name
    )
    Write-Information "Expanding '$Path' to '$Destination'"
    & $7zPath $7zArgs | Out-Null
}

enum BuildKind
{
    LocalBuild
    PullRequestBuild
    CiBuild
    ReleaseBuild
}

function Get-CurrentBuildKind
{
<#
.SYNOPSIS
    Determines the kind of build for the current environment

.DESCRIPTION
    This function retrieves environment values set by various automated builds
    to determine the kind of build the environment is for. The return is one of
    the [BuildKind] enumeration values:

    | Name             | Description |
    |------------------|-------------|
    | LocalBuild       | This is a local developer build (e.g. not an automated build)
    | PullRequestBuild | This is a build from a PullRequest with untrusted changes, so build should limit the steps appropriately |
    | CiBuild          | This build is from a Continuous Integration (CI) process, usually after a PR is accepted and merged to the branch |
    | ReleaseBuild     | This is an official release build, the output is ready for publication (Automated builds may use this to automatically publish) |
#>
    [OutputType([BuildKind])]
    param()

    $currentBuildKind = [BuildKind]::LocalBuild

    # IsAutomatedBuild is the top level gate (e.g. if it is false, all the others must be false)
    $isAutomatedBuild = [System.Convert]::ToBoolean($env:CI) `
                        -or [System.Convert]::ToBoolean($env:APPVEYOR) `
                        -or [System.Convert]::ToBoolean($env:GITHUB_ACTIONS)

    if( $isAutomatedBuild )
    {
        # PR and release builds have externally detected indicators that are tested
        # below, so default to a CiBuild (e.g. not a PR, And not a RELEASE)
        $currentBuildKind = [BuildKind]::CiBuild

        # IsPullRequestBuild indicates an automated buddy build and should not be trusted
        $isPullRequestBuild = $env:GITHUB_BASE_REF -or $env:APPVEYOR_PULL_REQUEST_NUMBER

        if($isPullRequestBuild)
        {
            $currentBuildKind = [BuildKind]::PullRequestBuild
        }
        else
        {
            if([System.Convert]::ToBoolean($env:APPVEYOR))
            {
                $isReleaseBuild = $env:APPVEYOR_REPO_TAG
            }
            elseif([System.Convert]::ToBoolean($env:GITHUB_ACTIONS))
            {
                $isReleaseBuild = $env:GITHUB_REF -like 'refs/tags/*'
            }

            if($isReleaseBuild)
            {
                $currentBuildKind = [BuildKind]::ReleaseBuild
            }
        }
    }

    return $currentBuildKind
}

function Get-GitHubReleases($org, $project)
{
<#
.SYNOPSIS
    Gets a collection of the GitHub releases for a project

.DESCRIPTION
    This function retrieves a collection of releases from a given GitHub organization and project.
    The result is a collection of GitHub releases as JSON data.

.PARAMETER org
    GitHub organization name that owns the project

.PARAMETER project
    GitHub project to retrieve releases from
#>
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    $releases = Invoke-RestMethod -Uri "https://api.github.com/repos/$org/$project/releases"
    foreach($r in $releases)
    {
        $r
    }
}

function Get-GitHubTaggedRelease($org, $project, $tag)
{
<#
.SYNOPSIS
    Gets a specific tagged release for a GitHub project

.DESCRIPTION
    This function retrieves a single tagged release from a given GitHub organization and project.
    The result is a GitHub release as JSON data.

.PARAMETER org
    GitHub organization name that owns the project

.PARAMETER project
    GitHub project to retrieve releases from

.PARAMETER tag
    Tag to find the specific release for
#>

    Get-GithubReleases $org $project | ?{$_.tag_name -eq $tag}
}

function Invoke-DotNetTest($buildInfo, $projectRelativePath)
{
<#
.SYNOPSIS
    Invokes specified .NET tests for a project

.DESCRIPTION
    This invokes 'dotnet.exe test ...' for the relative project path. The absolute path for the test to
    run is derived from the buildInfo parameter.

.PARAMETER buildInfo
    Hashtable of properties for the build. This function only requires two properties: "RepoRootPath", which is the Root
    of the repository this build is for and "SrcRootPath" that refers to the root of the source code of the repository. The
    relative project path is combined with the "RepoRootPath" to get the absolute path of the project to test. Additionally,
    there must be an 'x64.runsettings' file in the "SrcRootPath" to configure the proper settings for an x64 run.

.PARAMETER projectRelativePath
    Relative path to the project to test. The absolute path is computed by combining $buildInfo['RepoRootPath'] with the relative
    path provided.
#>
    $testProj = Join-Path $buildInfo['RepoRootPath'] $projectRelativePath
    $runSettings = Join-Path $buildInfo['SrcRootPath'] 'x64.runsettings'
    $result = dotnet test $testProj -s $runSettings --no-build --no-restore --logger "trx" -r $buildInfo['TestResultsPath'] `
            | Out-String
    Write-Information $result
    return $LASTEXITCODE -ne 0
}

function Get-BuildVersionXML
{
<#
.SYNOPSIS
    Retrieves the contents of the BuildVersion.XML file in the RepoRootPath

.DESCRIPTION
    Reads the contents of the BuildVersion.xml file and returns it as XML
    for additional processing.

.PARAMETER buildInfo
    Hashtable containing Information about the repository and build. This function
    requires the presence of a 'RepoRootPath' property to indicate the root of the
    repository containing the BuildVersion.xml file.
#>
    [OutputType([xml])]
    param ($buildInfo)

    return [xml](Get-Content (Join-Path $buildInfo['RepoRootPath'] 'BuildVersion.xml'))
}

function Get-BuildVersionTag
{
<#
.SYNOPSIS
    Retrieves the git tag name to apply for this build.

.DESCRIPTION
    Reads the contents of the BuildVersion.xml file and generates a git
    release tag name for the current build.

    This is a standalone function instead of a property on the build
    information Hashtable so that it is always dynamically evaluated
    based on the current contents of the BuildVersion.XML file as that
    is generally updated when this is needed.

.PARAMETER buildInfo
    Hashtable containing Information about the repository and build. This function
    requires the presence of a 'RepoRootPath' property to indicate the root of the
    repository containing the BuildVersion.xml file.
#>
    [OutputType([string])]
    Param($buildInfo)

    # determine release tag from the build version XML file in the branch
    $buildVersionXml = (Get-BuildVersionXML $buildInfo)
    $buildVersionData = $buildVersionXml.BuildVersionData
    $preReleaseSuffix=""
    if($buildVersionData.PSObject.Properties['PreReleaseName'])
    {
        $preReleaseSuffix = "-$($buildVersionData.PreReleaseName)"
        if($buildVersionData.PSObject.Properties['PreReleaseNumber'])
        {
            $preReleaseSuffix += ".$($buildVersionData.PreReleaseNumber)"
            if($buildVersionData.PSObject.Properties['PreReleaseFix'])
            {
                $preReleaseSuffix += ".$($buildVersionData.PreReleaseFix)"
            }
        }
    }

    return "v$($buildVersionData.BuildMajor).$($buildVersionData.BuildMinor).$($buildVersionData.BuildPatch)$preReleaseSuffix"
}

function Initialize-CommonBuildEnvironment
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

.PARAMETER DefaultMsBuildVerbosity
    Default MSBuild verbosity for the console logger output, default value for this is 'Minimal'.

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
    | MsBuildLoggerArgs   | Array of MSBuild arguments, normally this only contains the Logger parameters for the console logger verbosity |
    | MSBuildInfo         | Information about the found version of MSBuild (from a call to Find-MSBuild) |
    | VersionTag          | Git tag name for this build if released |
#>
    # support common parameters
    [cmdletbinding()]
    Param([switch]$FullInit,
          [switch]$AllowVsPreReleases,
          $AddRepoSpecificPropertiesFunc,
          $DefaultMsBuildVerbosity="Minimal"
         )

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
        if($currentBuildKind -ne [BuildKind]::LocalBuild)
        {
            $env:BuildTime = (git show -s --format=%cI)
        }
        else
        {
            $env:BuildTime = ([System.DateTime]::UtcNow.ToString("o"))
        }
    }

    $msbuildInfo = Find-MSBuild -AllowVsPreReleases:$AllowVsPreReleases
    if( !$msbuildInfo['FoundOnPath'] )
    {
        $env:Path = "$env:Path;$($msbuildInfo['BinPath'])"
    }

    $buildInfo = Get-DefaultBuildPaths ([IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..')))
    if(![string]::IsNullOrEmpty($DefaultMsBuildVerbosity))
    {
        $buildInfo['MsBuildLoggerArgs'] = @("/clp:Verbosity=$DefaultMsBuildVerbosity")
    }
    else
    {
        $buildInfo['MsBuildLoggerArgs'] = @()
    }

    $buildInfo['CurrentBuildKind'] = $currentBuildKind
    $buildInfo['MSBuildInfo'] = $msbuildInfo
    $buildInfo['VersionTag'] = Get-BuildVersionTag $buildInfo
    return $buildInfo
}

function Show-FullBuildInfo
{
<#
.SYNOPSIS
    Displays details of the build information and environment to the information and verbose streams

.PARAMETER buildInfo
    The build information Hashtable for the build. This normally contains the standard and repo specific
    properties so that the full details are available in logs.

.DESCRIPTION
    This function displays all the properties of the buildinfo to the information stream. Additionally,
    details of the current PATH, the .NET SDKs and runtimes installed is logged to the Verbose stream.
#>
    Param($buildInfo)

    Write-Information 'Build Info:'
    Write-Information ($buildInfo | Format-Table | Out-String)

    Write-Information "MSBUILD:`n$($buildInfo['MsbuildInfo'] | Format-Table -AutoSize | Out-String)"
    Write-Information (dir env:Is* | Format-Table -Property Name, value | Out-String)
    Write-Information (dir env:GITHUB* | Format-Table -Property Name, value | Out-String)
    Write-Information "BuildKind: $($buildInfo['CurrentBuildKind'])"
    Write-Information "CiBuildName: $env:CiBuildName"
    Write-Verbose 'PATH:'
    $($env:Path -split ';') | %{ Write-Verbose $_ }

    Write-Verbose ".NET Runtimes:"
    Write-Verbose (dotnet --list-runtimes | Out-String)

    Write-Verbose ".NET SDKs:"
    Write-Verbose (dotnet --list-sdks | Out-String)
}

