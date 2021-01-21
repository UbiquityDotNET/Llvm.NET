# Project/Repo specific extensions to common support
using module 'PSModules\CommonBuild\CommonBuild.psd1'

Set-StrictMode -version 3.0

$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

function Get-BuildInformation($DefaultVerbosity='Minimal')
{
    $buildInfo = Get-DefaultBuildPaths $PSScriptRoot
    $buildInfo['LlvmLibsRoot'] = Join-Path $PSScriptRoot 'llvm'
    $buildInfo['LlvmVersion'] = "10.0.0"
    $buildInfo['MsBuildLoggerArgs'] = @("/clp:Verbosity=$DefaultVerbosity")
    $buildInfo['LlvmLibsPackageReleaseName'] = "$($buildInfo['LlvmVersion'])-msvc-16.5"
    return $buildInfo
}

function Initialize-BuildEnvironment
{
    # support common parameters
    [cmdletbinding()]
    Param([switch]$FullInit, [switch]$AllowVsPreReleases)

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

    $buildInfo = Get-BuildInformation

    if($FullInit)
    {
        Write-Information 'Build Info:'
        Write-Information ($buildInfo | Format-Table | Out-String)

        Write-Information "MSBUILD:`n$($msbuildInfo | Format-Table -AutoSize | Out-String)"
        Write-Information (dir env:Is* | Format-Table -Property Name, value | Out-String)
        Write-Information (dir env:GITHUB* | Format-Table -Property Name, value | Out-String)
        Write-Information "BuildKind: $currentBuildKind"
        Write-Information "CiBuildName: $env:CiBuildName"
        Write-Verbose 'PATH:'
        $($env:Path -split ';') | %{ Write-Verbose $_ }

        Write-Verbose ".NET Runtimes:"
        Write-Verbose (dotnet --list-runtimes | Out-String)

        Write-Verbose ".NET SDKs:"
        Write-Verbose (dotnet --list-sdks | Out-String)
    }

    return $buildInfo
}

