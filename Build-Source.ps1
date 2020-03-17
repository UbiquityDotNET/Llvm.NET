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
