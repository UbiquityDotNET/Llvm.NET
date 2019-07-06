function Find-OnPath
{
    [CmdletBinding()]
    Param( [Parameter(Mandatory=$True,Position=0)][string]$exeName)
    $path = where.exe $exeName 2>$null
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

function Find-VSInstance([switch]$PreRelease)
{
    if( !(Get-Module -ListAvailable VSSetup))
    {
        Install-Module VSSetup -Scope CurrentUser -Force | Out-Null
    }
    Get-VSSetupInstance -Prerelease:$PreRelease |
        Select-VSSetupInstance -Require 'Microsoft.Component.MSBuild' |
        select -First 1
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
        Write-Verbose "MSBuild not found attempting to locate VS installation"
        $vsInstall = Find-VSInstance -Prerelease:$AllowVsPreReleases
        if( !$vsInstall )
        {
            throw "MSBuild not found on PATH and No instances of VS found to use"
        }

        Write-Verbose "VS installation found: $vsInstall"
        $msBuildPath = [System.IO.Path]::Combine( $vsInstall.InstallationPath, 'MSBuild', '15.0', 'bin', 'MSBuild.exe')
        if(!(Test-Path -PathType Leaf $msBuildPath))
        {
            $msBuildPath = [System.IO.Path]::Combine( $vsInstall.InstallationPath, 'MSBuild', 'current', 'bin', 'MSBuild.exe')
        }

        $foundOnPath = $false
    }

    if( !(Test-Path -PathType Leaf $msBuildPath ) )
    {
        Write-Verbose 'MSBuild not found'
        return $null
    }

    Write-Verbose "MSBuild Found at: $msBuildPath"
    return @{ FullPath=$msBuildPath
              BinPath=[System.IO.Path]::GetDirectoryName( $msBuildPath )
              FoundOnPath=$foundOnPath
            }
}

function Invoke-msbuild([string]$project, [hashtable]$properties, [string[]]$targets, [string[]]$loggerArgs=@(), [string[]]$additionalArgs=@())
{
    $oldPath = $env:Path
    try
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
    finally
    {
        $env:Path = $oldPath
    }
}

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
    $buildPaths =  @{}
    $buildPaths.RepoRoot = $repoRoot
    $buildPaths.BuildOutputPath = Normalize-Path (Join-Path $repoRoot 'BuildOutput')
    $buildPaths.NuGetRepositoryPath = Normalize-Path (Join-Path $buildPaths.BuildOutputPath 'packages')
    $buildPaths.NuGetOutputPath = Normalize-Path (Join-Path $buildPaths.BuildOutputPath 'NuGet')
    $buildPaths.SrcRoot = Normalize-Path (Join-Path $repoRoot 'src')
    $buildPaths.LlvmLibsRoot = Normalize-Path (Join-Path $buildPaths.repoRoot 'llvm')
    $buildPaths.BuildExtensionsRoot = ([IO.Path]::Combine( $repoRoot, 'BuildExtensions') )
    $buildPaths.GenerateVersionProj = ([IO.Path]::Combine( $buildPaths.BuildExtensionsRoot, 'CommonVersion.csproj') )
    $buildPaths.DocsOutput = ([IO.Path]::Combine( $buildPaths.BuildOutputPath, 'docs') )
    return $buildPaths
}

function Get-BuildInformation($buildPaths)
{
    Write-Information "Restoring NuGet for $($buildPaths.GenerateVersionProj)"
    Invoke-MSBuild -Targets 'Restore' -Project $buildPaths.GenerateVersionProj -LoggerArgs ($msbuildLoggerArgs + @("/bl:GenerateVersion-Restore.binlog") )

    Write-Information "Generating version info from $($buildPaths.GenerateVersionProj)"
    Invoke-MSBuild -Targets 'GenerateVersionJson' -Project $buildPaths.GenerateVersionProj -LoggerArgs ($msbuildLoggerArgs + @("/bl:GenerateVersion-Build.binlog") )

    $semVer = get-content (Join-Path $buildPaths.BuildOutputPath GeneratedVersion.json) | ConvertFrom-Json

    return @{ FullBuildNumber = $semVer.FullBuildNumber
              PackageVersion = $semVer.FullBuildNumber
              FileVersionMajor = $semVer.FileVersionMajor
              FileVersionMinor = $semVer.FileVersionMinor
              FileVersionBuild = $semVer.FileVersionBuild
              FileVersionRevision = $semver.FileVersionRevision
              FileVersion= "$($semVer.FileVersionMajor).$($semVer.FileVersionMinor).$($semVer.FileVersionBuild).$($semVer.FileVersionRevision)"
              LlvmVersion = "8.0.0" # TODO: Figure out how to extract this from the llvmlibs download
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
