Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases
)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -AllowVsPreReleases:$AllowVsPreReleases

    .\Move-LlvmBuild.ps1

    $msBuildProperties = @{ Configuration = $Configuration
        LlvmVersion = $buildInfo['LlvmVersion']
    }

    # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
    # and PackageReference isn't supported for native projects... [Sigh...]
    Write-Information "Restoring LibLLVM"
    Invoke-NuGet restore 'src\Interop\LibLLVM\LibLLVM.vcxproj'

    Write-Information "Building LibLLVM"
    $libLLVMBinLogPath = Join-Path $buildInfo['BinLogsPath'] LibLLVM-Build.binlog
    Invoke-MSBuild -Targets 'Build' -Project 'src\Interop\LibLLVM\LibLLVM.vcxproj' -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$libLLVMBinLogPath") )

}
finally
{
    popd
    $env:Path = $oldPath
}
