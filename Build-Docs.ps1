Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClone = (!([System.Convert]::ToBoolean($env:IsAutomatedBuild)))
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
    $msBuildProperties = @{ Configuration = $Configuration
                            LlvmVersion = $buildInfo['LlvmVersion']
                          }

    # clone docs output location so it is available as a destination for the Generated docs content
    if(!$NoClone -and !(Test-Path (Join-Path $buildInfo['DocsOutput'] '.git') -PathType Container))
    {
        Write-Information "Cloning Docs repository"
        pushd BuildOutput -ErrorAction Stop
        try
        {
            git clone https://github.com/UbiquityDotNET/Llvm.NET.git -b gh-pages docs -q
            if( !$? )
            {
                throw "Git clone failed"
            }
        }
        finally
        {
            popd
        }
    }

    $docfxRestoreBinLogPath = Join-Path $buildInfo['BinLogsPath'] Ubiquity.NET.Llvm-docfx-Build.restore.binlog
    $docfxBinLogPath = Join-Path $buildInfo['BinLogsPath'] Ubiquity.NET.Llvm-docfx-Build.binlog

    # DocFX.console build support is peculiar and a bit fragile, It requires a separate restore path or it won't do anything for the build target.
    Write-Information "Restoring Docs Project"
    Invoke-MSBuild -Targets 'Restore' -Project docfx\Ubiquity.NET.Llvm.DocFX.csproj -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$docfxRestoreBinLogPath") )

    Write-Information "Building Docs Project"
    Invoke-MSBuild -Targets 'Build' -Project docfx\Ubiquity.NET.Llvm.DocFX.csproj -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$docfxBinLogPath") )
}
finally
{
    popd
    $env:Path = $oldPath
}
