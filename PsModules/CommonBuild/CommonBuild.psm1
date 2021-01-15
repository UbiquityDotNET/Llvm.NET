# Repository neutral common build support utilities

$InformationPreference = "Continue"

enum Platform { Windows; Linux; Mac }

function Get-Platform
{
<#
.SYNOPSIS
    Returns the platform we're currently running on one of Windows, Linux, and Mac

.NOTES
    This should work for both Windows PowerShell and PowerShell Core
#>
    if ($PSVersionTable.PSEdition -ne "Core")
    {
        Write-Debug "Using Windows PowerShell"
        return [Platform]::Windows
    }
    else 
    {
        if ($IsLinux)
        {
            Write-Debug "Using PowerShell Core on Linux"
            return [Platform]::Linux
        }
        elseif ($IsMacOS)
        {
            Write-Debug "Using PowerShell Core on Mac"
            return [Platform]::Mac
        }
        else 
        {
            Write-Debug "Using PowerShell Core on Windows"
            return [Platform]::Windows
        }
    }
}

function Ensure-PathExists
{
    param([Parameter(Mandatory=$true, ValueFromPipeLine)]$path)
    New-Item -ItemType Directory -Force -ErrorAction SilentlyContinue $path | Out-Null
}

function Get-DefaultBuildPaths([string]$repoRoot)
{
<#
.SYNOPSIS
    Gets the default set of paths for a build

.DESCRIPTION
    This function initializes a hash table with the default paths for a build. This
    allows for standardization of build output locations etc... across builds. The
    values set are as follows:

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

    if ($env:DROP_NATIVE) {
        $buildPaths["NativeXplat"] = $env:DROP_NATIVE
    } else {
        $buildPaths["NativeXplat"] = Join-Path $buildOutputPath .. xplat
    }

    $buildPaths.GetEnumerator() | %{ Ensure-PathExists $_.Value }
    return $buildPaths
}

function Update-Submodules
{
<#
.SYNOPSIS
    Updates Git submodules
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
    This is a simple wrapper around the Get-Command applet to adjust the executable name for the platform
#>
    [CmdletBinding()]
    Param( [Parameter(Mandatory=$True,Position=0)][string]$exeName)
    $path = $null
    Write-Information "Searching for $exeName..."
    try
    {
        if ($global:Platform -ne [Platform]::Windows)
        {
            if ($exeName.EndsWith(".exe"))
            {
                $exeName = $exeName.Substring(0, $ExeName.Length - 4)
            }
        }
        $path = (Get-Command -Name $exeName).Source
    }
    catch
    {}
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
    This will attempt to find Nuget on the current path, and if not found will download the latest
    version from NuGet.org before running the command.
#>
    $buildPaths = Get-DefaultBuildPaths ([IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..' '..')))
    $NuGetPaths = Find-OnPath nuget -ErrorAction Continue
    if( !$NuGetPaths )
    {
        $nugetToolsPath = Join-Path $buildPaths['ToolsPath'] 'nuget'
        if( !(Test-Path $nugetToolsPath))
        {
            # Download it from official nuget release location
            Write-Verbose "Downloading nuget to $nugetToolsPath"
            # ToDo: Need a path to Unix nuget...
            Invoke-WebRequest -UseBasicParsing -Uri https://dist.NuGet.org/win-x86-commandline/latest/NuGet.exe -OutFile $nugetToolsPath
        }

        $env:Path = "$env:Path;$($buildPaths['ToolsPath'])"
    }
    Write-Information "nuget $args"
    nuget $args
    $err = $LASTEXITCODE
    if($err -ne 0)
    {
        throw "Error running nuget: $err"
    }
}

function Find-VSInstance([switch]$PreRelease, $Version = '[15.0, 17.0)', [string[]]$requiredComponents = @('Microsoft.Component.MSBuild'))
{
<#
.SYNOPSIS
    Finds an installed VS instance

.PARAMETER PreRelease
    Indicates if the search should include pre-release versions of Visual Studio

.PARAMETER version
    The version range to search for. [Default is '[15.0, 17.0)']

.PARAMETER requiredComponents
    The set of required components to search for. [Default is an empty array]

.DESCRIPTION
    Returns $null if not running on Windows.
    Uses the official MS provided PowerShell module to find a VS instance. If the VSSetup
    module is not loaded it is loaded first. If it isn't installed, then the module is installed.
#>
    $plat = Get-Platform
    Write-Debug "Running on $plat"

    if ($plat -eq [Platform]::Windows)
    {
        $forceModuleInstall = [System.Convert]::ToBoolean($env:IsAutomatedBuild)
        $existingModule = Get-InstalledModule -ErrorAction SilentlyContinue VSSetup
        if(!$existingModule)
        {
            Write-Debug "Installing VSSetup module"
            Install-Module VsSetup -Scope CurrentUser -Force:$forceModuleInstall | Out-Null
        }
    
        Write-Debug "Looking for VS"
        $vs = Get-VsSetupInstance -Prerelease:$PreRelease |
                Select-VsSetupInstance -Product * -Version $Version -Require $requiredComponents |
                select -Last 1
        Write-Debug "Found $($vs)"
        return $vs
    }
    else 
    {
        Write-Debug "Not on Windows, no VS"
        return $null
    }
}

function Find-MSBuild([switch]$AllowVsPreReleases)
{
    $foundOnPath = $true

    $plat = Get-Platform
    if ($plat -ne [Platform]::Windows)
    {
        Write-Debug "On Linux or Mac, using dotnet msbuild"
        $versionInfo = & dotnet msbuild -version
        return @{ FullPath="dotnet"
                  AdditionalArgs=@("msbuild")
                  FoundOnPath=$true
                  Version = $versionInfo[-1]
                }
    }

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
            $msBuildPath = [System.IO.Path]::Combine( $vsInstall.InstallationPath, 'MSBuild', 'Current', 'Bin', 'MSBuild.exe')
        }

        $foundOnPath = $false
    }

    if( !(Test-Path -PathType Leaf $msBuildPath ) )
    {
        throw 'MSBuild not found'
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

    $msbuild = Find-MSBuild -AllowVsPreReleases
    $msbuildArgs = $msbuild.AdditionalArgs + $msbuildArgs
    Write-Information "$($msbuild.FullPath) $($msbuildArgs -join ' ')"
    . $msbuild.FullPath @msbuildArgs
    if($LASTEXITCODE -ne 0)
    {
        throw "Error running msbuild for '$project': $LASTEXITCODE"
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
    | ReleaseBuild     | This is an official release build, the output ready for publication (Automated builds may use this to automatically publish) |
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
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    $releases = Invoke-RestMethod -Uri "https://api.github.com/repos/$org/$project/releases"
    foreach($r in $releases)
    {
        $r
    }
}

function Get-GitHubTaggedRelease($org, $project, $tag)
{
    Get-GithubReleases $org $project | ?{$_.tag_name -eq $tag}
}

function Invoke-DotNetTest($buildInfo, $projectRelativePath, $configuration)
{
    if ([string]::IsNullOrEmpty($configuration)) {
        $configuration = "Release"
    }
    $testProj = Join-Path $buildInfo['RepoRootPath'] $projectRelativePath
    $runSettings = Join-Path $buildInfo['SrcRootPath'] 'x64.runsettings'
    dotnet test $testProj -v m -s $runSettings --logger trx -c $configuration
    if ($LASTEXITCODE -ne 0) {
        throw "'dotnet test $testproj' exited with code: $LASTEXITCODE"
    }
}

function Get-BuildVersionXML
{
    [OutputType([xml])]
    param ()

    [xml]$buildVersionXml = Get-Content ([System.IO.Path]::Combine($PSScriptRoot, '..', '..', 'BuildVersion.xml'))
    return $buildVersionXml
}

function Get-BuildVersionTag
{
    # determine release tag from the build version XML file in the branch
    Param([xml]$buildVersionXml = (Get-BuildVersionXML))
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
