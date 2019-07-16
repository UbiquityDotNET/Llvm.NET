Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClone = (!($env:APPVEYOR)),
    [Parameter(ParameterSetName='FullBuild')]
    $BuildInfo
)

. .\buildutils.ps1

# Main Script entry point -----------
pushd $PSScriptRoot
$oldPath = $env:Path
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"
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

    $buildPaths = Get-BuildPaths $PSScriptRoot

    md $buildPaths.NuGetOutputPath -ErrorAction SilentlyContinue| Out-Null

    if(!$BuildInfo)
    {
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

    # DocFX.console build support is peculiar and a bit fragile, It requires a separate restore path or it won't do anything for the build target.
    Write-Information "Restoring Docs Project"
    Invoke-MSBuild -Targets 'Restore' -Project docfx\Llvm.NET.DocFX.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET-docfx-Build.binlog") )

    Write-Information "Building Docs Project"
    Invoke-MSBuild -Targets 'Build' -Project docfx\Llvm.NET.DocFX.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET-docfx-Build.binlog") )
}
finally
{
    popd
    $env:Path = $oldPath
}
