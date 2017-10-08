# Unfortunately, just getting DOCFX to successfully build requires a lot of undocumented
# workarounds for several bugs. Most of the problems come from it's use of MSBuild and
# Roslyn causing build conflicts with DotNet and VS build. It is beyond me why a doc
# generator tool needs to build a project, let alone why it needs to include the Roslyn
# Compiler too... [sigh]...
#
# This script will manually install the packages needed to run DOCFX and the memberpage
# plug-in used for the documentation. The script then creates a VCVARS environment to run
# DOCFX to work around the dependency and conflict problems with DOCFX and CoreCLR projects.

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
        if( !(Test-Path $nugetToolsPath))
        {
            # Download it from official NuGet release location
            Write-Verbose "Downloading Nuget.exe to $nugetToolsPath"
            Invoke-WebRequest -UseBasicParsing -Uri https://dist.NuGet.org/win-x86-commandline/latest/NuGet.exe -OutFile $nugetToolsPath
            Write-Verbose "Adding tools folder to path fro Nuget.exe"
        }
        $env:Path = "$env:Path;$(Join-Path $PSSCriptRoot 'Tools')"
    }
    Write-Information "$NugetPath"
    Write-Information "NuGet $args"
    NuGet.exe $args
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
    Install-Module VSSetup -Scope CurrentUser -Force | Out-Null
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

#--- Start of main script

if( !( Test-Path -PathType Container tools ) )
{
    md tools | out-null
}

Write-Information "Initializing VsEnv"
Initialize-VCVars

if(!(Test-Path 'tools\memberpage\content' -PathType Container))
{
    Write-Information "Fetching memberpage plugin and content"
    Invoke-NuGet install memberpage -ExcludeVersion -OutputDirectory tools
}

$docfxPath = Find-OnPath docfx.exe -ErrorAction Continue
if(!$docfxPath)
{
    $docfxPath = Join-Path $PSScriptRoot 'tools'
    Invoke-NuGet install docfx.console -Verbosity quiet -ExcludeVersion -OutputDirectory $docfxPath
    $docfxPath = Join-Path $docfxPath 'docfx.console\tools'
    if( ( $env:Path -split ';' ) -inotcontains $docfxPath )
    {
        $env:Path = "$env:Path;$docfxPath"
    }
}

Invoke-Nuget Install msdn.4.5.2 -ExcludeVersion -PreRelease -OutputDirectory tools -Verbosity quiet

if( !(Test-Path ".\BuildOutput\docs\.git" -PathType Container))
{
    Write-Information "Cloning Docs repo"
    git clone https://github.com/UbiquityDotNET/Llvm.NET.git -b gh-pages BuildOutput\docs -q
}

# DOCFX is inconsistent on relative paths in the docfx.json file
# (i.e. Metadata[x].src[y].src is relative to the docfx.json file
# but Metadata[x].dest is relative to the current directory.) So,
# workaround the inconsistency by switching to the directory with
# the DOCFX file so the difference is not relevant.
pushd docfx
try
{
    Write-Information "Generating docs"
    docfx
}
finally
{
    popd
}
