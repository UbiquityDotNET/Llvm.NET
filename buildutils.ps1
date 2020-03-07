function Find-OnPath
{
    [CmdletBinding()]
    Param( [Parameter(Mandatory=$True,Position=0)][string]$exeName)

    $path = where.exe $exeName 2>$null | select -First 1
    if(!$path)
    {
        Write-Verbose "'$exeName' not found on PATH"
    }
    else
    {
        Write-Verbose "Found: '$path'"
    }
    return $path
}

# invokes NuGet.exe, handles downloading it to the script root if it isn't already on the path
function Invoke-NuGet
{
    $NuGetPath = Find-OnPath NuGet.exe -ErrorAction Continue
    if( !$NuGetPath )
    {
        $nugetToolsPath = "$PSScriptRoot\Tools\NuGet.exe"
        if( !(Test-Path -PathType Leaf $nugetToolsPath))
        {
            if(!(Test-Path -PathType Container "$PSScriptRoot\Tools" ) )
            {
                md "$PSScriptRoot\Tools" | Out-Null
            }

            # Download it from official NuGet release location
            Write-Verbose "Downloading Nuget.exe to $nugetToolsPath"
            Invoke-WebRequest -UseBasicParsing -Uri https://dist.NuGet.org/win-x86-commandline/latest/NuGet.exe -OutFile $nugetToolsPath
            Write-Verbose "Adding tools folder to path for Nuget.exe"
        }
        $env:Path = "$env:Path;$(Join-Path $PSSCriptRoot 'Tools')"
    }

    Write-Information "$NugetPath"
    Write-Information "NuGet $args"
    NuGet $args
    $err = $LASTEXITCODE
    if($err -ne 0)
    {
        throw "Error running NuGet: $err"
    }
}

function Get-CmdEnvironment ($cmd, $Arguments)
{
    $retVal = @{}
    Write-Verbose "Running [`"$cmd`" $Arguments >nul & set] to get environment variables"
    $envOut =  cmd /c "`"$cmd`" $Arguments >nul & set"
    foreach( $line in $envOut )
    {
        $name, $value = $line.split('=');
        $retVal.Add($name, $value)
    }
    return $retVal
}

function Merge-Environment( [hashtable]$OtherEnv, [string[]]$IgnoreNames )
{
<#
.SYNOPSIS
    Merges the name value pairs of a hash table into the current environment

.PARAMETER OtherEnv
    Hash table containing name value pairs to add to the environment

.PARAMETER IgnoreNames
    Names of properties in OtherEnv to ignore
.NOTES
    Standard system variables are always ignored and are blocked from merging
#>
    $SystemVars = @('COMPUTERNAME',
                    'USERPROFILE',
                    'HOMEPATH',
                    'LOCALAPPDATA',
                    'PSModulePath',
                    'PROCESSOR_ARCHITECTURE',
                    'CommonProgramFiles(x86)',
                    'ProgramFiles(x86)',
                    'PROCESSOR_LEVEL',
                    'LOGONSERVER',
                    'SystemRoot',
                    'SESSIONNAME',
                    'ALLUSERSPROFILE',
                    'PUBLIC',
                    'APPDATA',
                    'PROCESSOR_REVISION',
                    'USERNAME',
                    'CommonProgramW6432',
                    'CommonProgramFiles',
                    'OS',
                    'USERDOMAIN_ROAMINGPROFILE',
                    'PROCESSOR_IDENTIFIER',
                    'ComSpec',
                    'SystemDrive',
                    'ProgramFiles',
                    'NUMBER_OF_PROCESSORS',
                    'ProgramData',
                    'ProgramW6432',
                    'windir',
                    'USERDOMAIN'
                   )
    $IgnoreNames += $SystemVars
    $otherEnv.GetEnumerator() | ?{ !($ignoreNames -icontains $_.Name) } | %{ Set-Item -Path "env:$($_.Name)" -value $_.Value }
}

function Find-VSInstance([switch]$PreRelease, [string[]]$Requires = @('Microsoft.Component.MSBuild'))
{
    if( !(Get-Module -ListAvailable VSSetup))
    {
        Install-Module VSSetup -Scope CurrentUser -Force | Out-Null
    }
    Get-VSSetupInstance -Prerelease:$PreRelease |
        Select-VSSetupInstance -Require $Requires -Latest
}

function Initialize-VCVars($vsInstance = (Find-VSInstance))
{
    if(!$env:VCINSTALLDIR)
    {
        Write-Verbose "VS Install: $vsInstance.InstallationPath"
        if($vsInstance)
        {
            $vcEnv = Get-CmdEnvironment (Join-Path $vsInstance.InstallationPath 'Common7\Tools\VsDevCmd.bat')
            Merge-Environment $vcEnv @('Prompt')
        }
        else
        {
            Write-Error "VisualStudio instance not found"
        }
    }
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

function Invoke-msbuild([string]$project, [hashtable]$properties, $targets, $loggerArgs=@(), $additionalArgs=@())
{
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

# ensures a full path that includes a terminating separator
function Normalize-Path([string]$path)
{
    $path = [System.IO.Path]::GetFullPath($path)
    if( !$path.EndsWith([System.IO.Path]::DirectorySeparatorChar) )
    {
        $path += [System.IO.Path]::DirectorySeparatorChar
    }
    return $path
}

function Get-BuildPaths([string]$repoRoot)
{
    $BuildPaths =  @{}
    $BuildPaths.RepoRoot = $repoRoot
    $BuildPaths.BuildOutputPath = Normalize-Path (Join-Path $repoRoot 'BuildOutput')
    $BuildPaths.NuGetRepositoryPath = Normalize-Path (Join-Path $BuildPaths.BuildOutputPath 'packages')
    $BuildPaths.NuGetOutputPath = Normalize-Path (Join-Path $BuildPaths.BuildOutputPath 'NuGet')
    $BuildPaths.SrcRoot = Normalize-Path (Join-Path $repoRoot 'src')
    $BuildPaths.LlvmLibsRoot = Normalize-Path (Join-Path $BuildPaths.repoRoot 'llvm')
    $BuildPaths.DocsOutput = ([IO.Path]::Combine( $BuildPaths.BuildOutputPath, 'docs') )
    $BuildPaths.BinLogsPath = Normalize-Path (Join-Path $BuildPaths.BuildOutputPath 'BinLogs')
    $BuildPaths.TestResultsPath = Join-Path $BuildPaths.BuildOutputPath 'Test-Results'
    return $BuildPaths
}

function Parse-BuildVersionXml
{
    $repoVersionInfo = ((Get-Content .\BuildVersion.xml) -as [xml]).BuildVersionData

    # force prerelese number and fix values if not supporte
    if([string]::IsNullOrWhiteSpace($repoVersionInfo['PreReleaseName']))
    {
        $repoVersionInfo['PreReleaseNumber'] = 0
    }

    if(($repoVersionInfo['PreReleaseNumber'] -as [int]) -eq 0)
    {
        $repoVersionInfo['PreReleaseFix'] = 0
    }
}

function Get-BuildInformation($BuildPaths, $DefaultVerbosity='Minimal')
{
    $msbuildLoggerArgs = @("/clp:Verbosity=$DefaultVerbosity")

    return @{
              LlvmVersion = "8.0.0" # TODO: Figure out how to extract this from the llvmlibs download
              MsBuildLoggerArgs = $msbuildLoggerArgs
            }
}

function ConvertTo-PropertyList([hashtable]$table)
{
    (($table.GetEnumerator() | %{ "$($_.Key)=$($_.Value)" }) -join ';')
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

Function Expand-Archive([string]$Path, [string]$Destination) {
    $7zPath = Find-7Zip
    $7zArgs = @(
        'x'  # eXtract with full path
        '-y' # auto apply Yes for all prompts
        "`"-o$($Destination)`""  # Output directory
        "`"$($Path)`"" # 7z file name
    )
    & $7zPath $7zArgs
}

function Find-7Zip()
{
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

function Install-LlvmLibs($destPath, $llvmversion, $compiler, $compilerversion)
{
    if(!(Test-Path -PathType Container $destPath))
    {
        if(!(Test-Path -PathType Container Downloads))
        {
            md Downloads | Out-Null
        }

        $releaseName = "$llvmVersion-$compiler-$compilerVersion"
        $localLlvmLibs7zPath = Join-Path 'Downloads' "llvm-libs-$releaseName.7z"
        if( !( test-path -PathType Leaf $localLlvmLibs7zPath ) )
        {
            $release = Get-GitHubTaggedRelease UbiquityDotNet 'Llvm.Libs' "v$releaseName"
            if($release)
            {
                $asset = (Get-GitHubTaggedRelease UbiquityDotNet 'Llvm.Libs' "v$releaseName").assets[0]
                Invoke-WebRequest -UseBasicParsing -Uri $asset.browser_download_url -OutFile $localLlvmLibs7zPath
            }
            else
            {
                throw "Release 'v$releaseName' not found!"
            }
        }

        Expand-Archive $localLlvmLibs7zPath $destPath
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
    # force invalid env values to null
    $env:CurrentBuildKind = $currentBuildKind = $env:CurrentBuildKind -as [BuildKind]

    if(!$currentBuildKind)
    {
        $currentBuildKind = [BuildKind]::LocalBuild

        # IsAutomatedBuild is the top level gate (e.g. if it is false, all the others must be false)
        $isAutomatedBuild = [System.Convert]::ToBoolean($env:IsAutomatedBuild) `
                            -or $env:CI `
                            -or $env:APPVEYOR `
                            -or $env:GITHUB_ACTIONS

        if( $isAutomatedBuild )
        {
            # IsPullRequestBuild indicates an automated buddy build and should not be trusted
            $isPullRequestBuild = [System.Convert]::ToBoolean($env:IsPullRequestBuild)
            if(!$isPullRequestBuild)
            {
                $isPullRequestBuild = $env:GITHUB_BASE_REF -or $env:APPVEYOR_PULL_REQUEST_NUMBER
            }

            if($isPullRequestBuild)
            {
                $currentBuildKind = [BuildKind]::PullRequestBuild
            }
            else
            {
                $isReleaseBuild = [System.Convert]::ToBoolean($env:IsReleaseBuild)
                if(!$isReleaseBuild -and !$isPullRequestBuild)
                {
                    if($env:APPVEYOR)
                    {
                        $isReleaseBuild = $env:APPVEYOR_REPO_TAG
                    }
                    else
                    {
                        $isReleaseBuild = $env:GITHUB_REF -like 'refs/tags/*'
                    }
                }

                if($isReleaseBuild)
                {
                    $currentBuildKind = [BuildKind]::ReleaseBuild
                }
            }
        }
    }

    return $currentBuildKind
}

function Initialize-BuildEnvironment
{
    # support common parameters
    [cmdletbinding()]
    Param([switch]$FullInit)

    # Script code should ALWAYS use the global CurrentBuildKind
    $global:CurrentBuildKind = Get-CurrentBuildKind

    # set/reset legacy environment vars for non-script tools (i.e. msbuild.exe)
    $env:IsAutomatedBuild = $global:CurrentBuildKind -ne [BuildKind]::LocalBuild
    $env:IsPullRequestBuild = $global:CurrentBuildKind -eq [BuildKind]::PullRequestBuild
    $env:IsReleaseBuild = $global:CurrentBuildKind -eq [BuildKind]::ReleaseBuild

    switch($global:CurrentBuildKind)
    {
        ([BuildKind]::LocalBuild) { $env:CiBuildName = 'ZZZ' }
        ([BuildKind]::PullRequestBuild) { $env:CiBuildName = 'PRQ' }
        ([BuildKind]::CiBuild) { $env:CiBuildName = 'BLD' }
        ([BuildKind]::ReleaseBuild) { $env:CiBuildName = '' }
        default { throw "Invalid build kind" }
    }

    # get the ISO-8601 formatted time stamp of the HEAD commit or the current UTC time for local builds
    if(!$env:BuildTime -or $FullInit)
    {
        if($global:CurrentBuildKind -ne [BuildKind]::LocalBuild)
        {
            $env:BuildTime = (git show -s --format=%cI)
        }
        else
        {
            $env:BuildTime = ([System.DateTime]::UtcNow.ToString("o"))
        }
    }

    $msbuild = Find-MSBuild
    if( !$msbuild )
    {
        throw "MSBuild not found"
    }

    if( !$msbuild.FoundOnPath )
    {
        $env:Path = "$env:Path;$($msbuild.BinPath)"
    }

    if($FullInit -or !$global:BuildPaths -or !$global:BuildInfo)
    {
        $global:BuildPaths = Get-BuildPaths $PSScriptRoot
        $global:BuildInfo = Get-BuildInformation $BuildPaths
    }

    if($FullInit)
    {
        Write-Information 'Build Paths:'
        Write-Information ($global:BuildPaths | Format-Table | Out-String)

        Write-Information 'Build Info:'
        Write-Information ($global:BuildInfo | Format-Table | Out-String)

        Write-Information "MSBUILD:`n$($msbuild | Format-Table -AutoSize | Out-String)"
        Write-Information (dir env:Is* | Format-Table -Property Name, value | Out-String)
        Write-Information "BuildKind: $global:BuildKind"
        Write-Information 'PATH:'
        $($env:Path -split ';') | %{ Write-Information $_ }

        Write-Information ".NET Runtimes:"
        Write-Information (dotnet --list-runtimes | Out-String)

        Write-Information ".NET SDKs:"
        Write-Information (dotnet --list-sdks | Out-String)
    }

    Set-StrictMode -version 1.0
}

