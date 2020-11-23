Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
)

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment

    $packProperties = @{ version=$($buildInfo['PackageVersion'])
                         llvmversion=$($buildInfo['LlvmVersion'])
                         buildbinoutput=(Join-path $($buildInfo['BuildOutputPath']) 'bin')
                         configuration=$Configuration
                       }

    $msBuildProperties = @{ Configuration = $Configuration
                            LlvmVersion = $buildInfo['LlvmVersion']
                          }

    $buildLogPath = Join-Path $buildInfo['BinLogsPath'] Ubiquity.NET.Llvm.binlog
    Write-Information "Building Ubiquity.NET.Llvm"
    Invoke-MSBuild -Targets 'Restore;Build' -Project src\Ubiquity.NET.Llvm.sln -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$buildLogPath") )
}
catch
{
    Write-Error $_.Exception.Message
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}

Write-Information "Done building LLVM"
