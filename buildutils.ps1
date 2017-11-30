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
        $msbuildArgs = @($project, "/m", "/nr:false", "/t:$($targets -join ';')") + $loggerArgs + $additionalArgs
        if( $properties )
        {
            $msbuildArgs += @( "/p:$(ConvertTo-PropertyList $properties)" ) 
        }

        Write-Information "msbuild $($msbuildArgs -join ' ')"
        msbuild $msbuildArgs
        if($LASTEXITCODE -ne 0)
        {
            throw "Error running msbuild: $LASTEXITCODE"
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
    $buildPaths.LibLLVMSrcRoot = Normalize-Path (Join-Path $buildPaths.SrcRoot 'LibLLVM')
    $buildPaths.BuildExtensionsRoot = ([IO.Path]::Combine( $repoRoot, 'BuildExtensions') )
    $buildPaths.GenerateVersionProj = ([IO.Path]::Combine( $buildPaths.BuildExtensionsRoot, 'CommonVersion.csproj') )
    return $buildPaths
}

function Get-BuildInformation($buildPaths)
{
    Write-Information "Restoring NuGet for $($buildPaths.GenerateVersionProj)"
    Invoke-MSBuild -Targets Restore -Project $buildPaths.GenerateVersionProj -LoggerArgs $msbuildLoggerArgs

    Write-Information "Computing Build information"
    Invoke-MSBuild -Targets GenerateVersionJson -Project $buildPaths.GenerateVersionProj -LoggerArgs $msbuildLoggerArgs

    $semVer = get-content (Join-Path $buildPaths.BuildOutputPath GeneratedVersion.json) | ConvertFrom-Json

    return @{ FullBuildNumber = $semVer.FullBuildNumber
              PackageVersion = $semVer.FullBuildNumber
              FileVersionMajor = $semVer.FileVersionMajor
              FileVersionMinor = $semVer.FileVersionMinor
              FileVersionBuild = $semVer.FileVersionBuild
              FileVersionRevision = $semver.FileVersionRevision
              FileVersion= "$($semVer.FileVersionMajor).$($semVer.FileVersionMinor).$($semVer.FileVersionBuild).$($semVer.FileVersionRevision)"
              LlvmVersion = "5.0.0"
            }
}

function ConvertTo-PropertyList([hashtable]$table)
{
    (($table.GetEnumerator() | %{ "$($_.Key)=$($_.Value)" }) -join ';')
}
