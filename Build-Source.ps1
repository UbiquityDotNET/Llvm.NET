Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [Parameter(ParameterSetName='FullBuild')]
    $BuildInfo
)

. .\buildutils.ps1

Initialize-BuildEnvironment

# Main Script entry point -----------
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    $msbuild = Find-MSBuild -AllowVsPrereleases:$AllowVsPreReleases
    if( !$msbuild )
    {
        throw "MSBuild not found"
    }

    if( !$msbuild.FoundOnPath )
    {
        $env:Path = "$env:Path;$($msbuild.BinPath)"
    }

    # setup standard MSBuild logging for this build
    $msbuildLoggerArgs = @('/clp:Verbosity=Minimal')

    if (Test-Path "C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll")
    {
        $msbuildLoggerArgs = $msbuildLoggerArgs + @("/logger:`"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll`"")
    }

    if(!$BuildInfo)
    {
        $buildPaths = Get-BuildPaths $PSScriptRoot
        $BuildInfo = Get-BuildInformation $buildPaths
    }

    if($env:APPVEYOR)
    {
        Write-Information "Updating APPVEYOR version: $($BuildInfo.FullBuildNumber)"
        Update-AppVeyorBuild -Version "$($BuildInfo.FullBuildNumber) [$([DateTime]::Now)]"
    }

    $packProperties = @{ version=$($BuildInfo.PackageVersion)
                         llvmversion=$($BuildInfo.LlvmVersion)
                         buildbinoutput=(normalize-path (Join-path $($buildPaths.BuildOutputPath) 'bin'))
                         configuration=$Configuration
                       }

    $msBuildProperties = @{ Configuration = $Configuration
                            FullBuildNumber = $BuildInfo.FullBuildNumber
                            PackageVersion = $BuildInfo.PackageVersion
                            FileVersionMajor = $BuildInfo.FileVersionMajor
                            FileVersionMinor = $BuildInfo.FileVersionMinor
                            FileVersionBuild = $BuildInfo.FileVersionBuild
                            FileVersionRevision = $BuildInfo.FileVersionRevision
                            FileVersion = $BuildInfo.FileVersion
                            LlvmVersion = $BuildInfo.LlvmVersion
                          }

    Write-Information "Build Parameters:"
    Write-Information ($BuildInfo | Format-Table | Out-String)

    # Download and unpack the LLVM libs if not already present, this doesn't use NuGet as the NuGet compression
    # is insufficient to keep the size reasonable enough to support posting to public galleries. Additionally, the
    # support for native lib projects in NuGet is tenuous at best. Due to various compiler version dependencies
    # and incompatibilities libs are generally not something published in a package. However, since the build time
    # for the libraries exceeds the time allowed for most hosted build services these must be pre-built for the
    # automated builds.
    Install-LlvmLibs $buildPaths.LlvmLibsRoot "8.0.0" "msvc" "15.9"

    .\Build-Interop.ps1 -BuildInfo $BuildInfo

    Write-Information "Restoring NuGet Packages for Llvm.NET"
    Invoke-MSBuild -Targets 'Restore;Build' -Project src\Llvm.NET.sln -Properties $msBuildProperties -LoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET.binlog") )
}
finally
{
    popd
    $env:Path = $oldPath
}
