Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClone = (!([bool]$env:IsAutomatedBuild)),
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
    if(!$BuildInfo)
    {
        $buildPaths = Get-BuildPaths $PSScriptRoot
        $BuildInfo = Get-BuildInformation $buildPaths
        if($env:APPVEYOR)
        {
            Update-AppVeyorBuild -Version $BuildInfo.FullBuildNumber
        }
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

    # clone docs output location so it is available as a destination for the Generated docs content
    if(!$NoClone -and !(Test-Path (Join-Path $buildPaths.DocsOutput '.git') -PathType Container))
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

    $docfxRestoreBinLogPath = Join-Path $buildPaths.BinLogsPath Llvm.NET-docfx-Build.restore.binlog
    $docfxBinLogPath = Join-Path $buildPaths.BinLogsPath Llvm.NET-docfx-Build.binlog

    # DocFX.console build support is peculiar and a bit fragile, It requires a separate restore path or it won't do anything for the build target.
    Write-Information "Restoring Docs Project"
    Invoke-MSBuild -Targets 'Restore' -Project docfx\Llvm.NET.DocFX.csproj -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:$docfxRestoreBinLogPath") )

    Write-Information "Building Docs Project"
    Invoke-MSBuild -Targets 'Build' -Project docfx\Llvm.NET.DocFX.csproj -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:$docfxBinLogPath") )
}
finally
{
    popd
    $env:Path = $oldPath
}
