# Repository neutral common build support utilities

function Ensure-PathExists
{
    param([Parameter(Mandatory=$true, ValueFromPipeLine)]$path)
    md -Force -ErrorAction SilentlyContinue $path | Out-Null
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

    $buildPaths.GetEnumerator() | %{ Ensure-PathExists $_.Value }
    return $buildPaths
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

        Write-Information "VS installation found: $($vsInstall | Format-List | Out-String)"
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
        Write-Error "Error running msbuild: $LASTEXITCODE"
    }
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

function Invoke-DotNetTest($buildInfo, $projectRelativePath, $configuration, $additionalArgs = @())
{
    # Blame mode for dotnet test will execute tests in sequence instead of parallel and on a crash
    # output Sequence.xml file that shows which tests were run before the crash to help with debugging.
    # This slows tests down but is very handy for determining which test is the source of an intermittent
    # crash.
    $blameMode = @("--blame")
    # $blameMode = @()

    if ([string]::IsNullOrEmpty($configuration)) {
        $configuration = "Release"
    }
    $testProj = Join-Path $buildInfo['RepoRootPath'] $projectRelativePath
    $runSettings = Join-Path $buildInfo['SrcRootPath'] 'x64.runsettings'
    dotnet test $testProj -v m -s $runSettings --logger trx -c $configuration @blameMode @additionalArgs
    if ($LASTEXITCODE -ne 0) {
        if ($blameMode.Count -gt 0) {
            Get-ChildItem (Join-Path $buildInfo['TestResultsPath'] '*' '*Sequence*.xml*') |`
                ForEach-Object {write-information $_.fullname ; Get-Content $_ ; write-information "__END__"}
        }
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
