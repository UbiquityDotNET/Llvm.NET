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
    $BuildPaths.BuildExtensionsRoot = ([IO.Path]::Combine( $repoRoot, 'BuildExtensions') )
    $BuildPaths.GenerateVersionProj = ([IO.Path]::Combine( $BuildPaths.BuildExtensionsRoot, 'CommonVersion.csproj') )
    $BuildPaths.DocsOutput = ([IO.Path]::Combine( $BuildPaths.BuildOutputPath, 'docs') )
    $BuildPaths.BinLogsPath = Normalize-Path (Join-Path $BuildPaths.BuildOutputPath 'BinLogs')
    $BuildPaths.TestResultsPath = Join-Path $BuildPaths.BuildOutputPath 'Test-Results'
    return $BuildPaths
}

function Get-BuildInformation($BuildPaths, $DefaultVerbosity='Minimal')
{
    $msbuildLoggerArgs = @("/clp:Verbosity=$DefaultVerbosity")

    if (Test-Path "C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll")
    {
        $msbuildLoggerArgs += "/logger:`"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll`""
    }

    Write-Information "Restoring NuGet for $($BuildPaths.GenerateVersionProj)"
    $binLogPath = Join-Path $BuildPaths.BinLogsPath GenerateVersion-Restore.binlog
    $ignored = Invoke-MSBuild -Targets 'Restore' -Project $BuildPaths.GenerateVersionProj -LoggerArgs ($msbuildLoggerArgs + @("/bl:$binLogPath") )

    Write-Information "Generating version info from $($BuildPaths.GenerateVersionProj)"
    $binLogPath = Join-Path $BuildPaths.BinLogsPath GenerateVersion-Restore.binlog
    $ignored = Invoke-MSBuild -Targets 'GenerateVersionJson' -Project $BuildPaths.GenerateVersionProj -LoggerArgs ($msbuildLoggerArgs + @("/bl:$binLogPath") )

    $semVer = get-content (Join-Path $BuildPaths.BuildOutputPath GeneratedVersion.json) | ConvertFrom-Json

    return @{ FullBuildNumber = $semVer.FullBuildNumber
              PackageVersion = $semVer.FullBuildNumber
              FileVersionMajor = $semVer.FileVersionMajor
              FileVersionMinor = $semVer.FileVersionMinor
              FileVersionBuild = $semVer.FileVersionBuild
              FileVersionRevision = $semver.FileVersionRevision
              FileVersion= "$($semVer.FileVersionMajor).$($semVer.FileVersionMinor).$($semVer.FileVersionBuild).$($semVer.FileVersionRevision)"
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
    $7zPath = Find7Zip
    $7zArgs = @(
        'x'  # eXtract with full path
        '-y' # auto apply Yes for all prompts
        "`"-o$($Destination)`""  # Output directory
        "`"$($Path)`"" # 7z file name
    )
    & $7zPath $7zArgs
}

function Find7Zip()
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

function Initialize-BuildEnvironment
{
    # support common parameters
    [cmdletbinding()]
    Param([switch]$FullInit)

    # IsAutomatedBuild is the top level gate (e.g. if it is false, all the others must be false)
    $global:IsAutomatedBuild = [System.Convert]::ToBoolean($env:IsAutomatedBuild) `
                               -or $env:CI `
                               -or $env:APPVEYOR `
                               -or $env:GITHUB_ACTIONS

    # IsPullRequestBuild indicates an automated buddy build and should not be trusted
    $global:IsPullRequestBuild = [System.Convert]::ToBoolean($env:IsPullRequestBuild)
    if(!$global:IsPullRequestBuild -and $global:IsAutomatedBuild)
    {
        $global:IsPullRequestBuild = $env:GITHUB_BASE_REF -or $env:APPVEYOR_PULL_REQUEST_NUMBER
    }

    $global:IsReleaseBuild = [System.Convert]::ToBoolean($env:IsReleaseBuild)
    if(!$global:IsReleaseBuild -and $global:IsAutomatedBuild -and !$global:IsPullRequestBuild)
    {
        if($env:APPVEYOR)
        {
            $global:IsReleaseBuild = $env:APPVEYOR_REPO_TAG
        }
        else
        {
            $global:IsReleaseBuild = $env:GITHUB_REF -like 'refs/tags/*'
        }
    }

    # set/reset environment vars for non-script tools (i.e. msbuild.exe)
    # Script code should ALWAYS use the globals as they don't require conversion to bool
    $env:IsAutomatedBuild = $global:IsAutomatedBuild
    $env:IsPullRequestBuild = $global:IsPullRequestBuild
    $env:IsReleaseBuild = $global:IsReleaseBuild

    $msbuild = Find-MSBuild
    if( !$msbuild )
    {
        throw "MSBuild not found"
    }

    if( !$msbuild.FoundOnPath )
    {
        $env:Path = "$env:Path;$($msbuild.BinPath)"
    }

    # for an automated build, get the ISO-8601 formatted time stamp of the HEAD commit
    if($global:IsAutomatedBuild -and !$env:BuildTime)
    {
        $env:BuildTime = (git show -s --format=%cI)
    }

    if($FullInit)
    {
        $global:BuildPaths = Get-BuildPaths $PSScriptRoot
        $global:BuildInfo = Get-BuildInformation $BuildPaths

        Write-Information 'Build Paths:'
        Write-Information ($global:BuildPaths | Format-Table | Out-String)

        Write-Information 'Build Info:'
        Write-Information ($global:BuildInfo | Format-Table | Out-String)

        Write-Information "MSBUILD:`n$($msbuild | Format-Table -AutoSize | Out-String)"
        Write-Information (dir env:Is* | Format-Table -Property Name, value | Out-String)
        Write-Information 'PATH:'
        $($env:Path -split ';') | %{ Write-Information $_ }

        Write-Information ".NET Runtimes:"
        Write-Information (dotnet --list-runtimes | Out-String)

        Write-Information ".NET SDKs:"
        Write-Information (dotnet --list-sdks | Out-String)
    }

    Set-StrictMode -version 1.0
}

