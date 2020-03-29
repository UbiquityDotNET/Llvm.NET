Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
)

. .\buildutils.ps1
$buildInfo = Initialize-BuildEnvironment

# Main Script entry point -----------
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    $packProperties = @{ version=$($buildInfo['PackageVersion'])
                         llvmversion=$($buildInfo['LlvmVersion'])
                         buildbinoutput=(Join-path $($buildInfo['BuildOutputPath']) 'bin')
                         configuration=$Configuration
                       }

    $msBuildProperties = @{ Configuration = $Configuration
                            LlvmVersion = $buildInfo['LlvmVersion']
                          }

    .\Build-Interop.ps1

    $buildLogPath = Join-Path $buildInfo['BinLogsPath'] Ubiquity.NET.Llvm.binlog
    Write-Information "Building Ubiquity.NET.Llvm"
    Invoke-MSBuild -Targets 'Restore;Build' -Project src\Ubiquity.NET.Llvm.sln -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$buildLogPath") )

    pushd $buildInfo['NuGetOutputPath']
    Compress-Archive -Force -Path *.* -DestinationPath (join-path $buildInfo['BuildOutputPath'] Nuget.Packages.zip)
}
finally
{
    popd
    $env:Path = $oldPath
}
