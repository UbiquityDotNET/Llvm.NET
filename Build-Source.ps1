Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
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
    $packProperties = @{ version=$($BuildInfo.PackageVersion)
                         llvmversion=$($BuildInfo.LlvmVersion)
                         buildbinoutput=(normalize-path (Join-path $($BuildPaths.BuildOutputPath) 'bin'))
                         configuration=$Configuration
                       }

    $msBuildProperties = @{ Configuration = $Configuration
                            LlvmVersion = $BuildInfo.LlvmVersion
                          }

    # Download and unpack the LLVM libs if not already present, this doesn't use NuGet as the NuGet compression
    # is insufficient to keep the size reasonable enough to support posting to public galleries. Additionally, the
    # support for native lib projects in NuGet is tenuous at best. Due to various compiler version dependencies
    # and incompatibilities libs are generally not something published in a package. However, since the build time
    # for the libraries exceeds the time allowed for most hosted build services these must be pre-built for the
    # automated builds.
    Install-LlvmLibs $BuildPaths.LlvmLibsRoot "8.0.0" "msvc" "15.9"

    .\Build-Interop.ps1 -BuildInfo $BuildInfo

    $buildLogPath = Join-Path $BuildPaths.BinLogsPath Ubiquity.NET.Llvm.binlog
    Write-Information "Building Ubiquity.NET.Llvm"
    Invoke-MSBuild -Targets 'Restore;Build' -Project src\Ubiquity.NET.Llvm.sln -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:$buildLogPath") )

    pushd $BuildPaths.NuGetOutputPath
    Compress-Archive -Force -Path *.* -DestinationPath (join-path $BuildPaths.BuildOutputPath Nuget.Packages.zip)
}
finally
{
    popd
    $env:Path = $oldPath
}
