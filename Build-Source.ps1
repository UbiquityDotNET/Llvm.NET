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
    $buildPaths = Get-BuildPaths $PSScriptRoot
    if(!$BuildInfo)
    {
        $BuildInfo = Get-BuildInformation $buildPaths
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

    # Download and unpack the LLVM libs if not already present, this doesn't use NuGet as the NuGet compression
    # is insufficient to keep the size reasonable enough to support posting to public galleries. Additionally, the
    # support for native lib projects in NuGet is tenuous at best. Due to various compiler version dependencies
    # and incompatibilities libs are generally not something published in a package. However, since the build time
    # for the libraries exceeds the time allowed for most hosted build services these must be pre-built for the
    # automated builds.
    Install-LlvmLibs $buildPaths.LlvmLibsRoot "8.0.0" "msvc" "15.9"

    .\Build-Interop.ps1 -BuildInfo $BuildInfo

    $buildLogPath = Join-Path $buildPaths.BinLogsPath Llvm.NET.binlog
    Write-Information "Restoring NuGet Packages for Llvm.NET"
    Invoke-MSBuild -Targets 'Restore;Build' -Project src\Llvm.NET.sln -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:$buildLogPath") )
}
finally
{
    popd
    $env:Path = $oldPath
}
