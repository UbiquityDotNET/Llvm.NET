Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClean
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

    $BuildInfo = Get-BuildInformation $buildPaths
    if($env:APPVEYOR)
    {
        Update-AppVeyorBuild -Version $BuildInfo.FullBuildNumber
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


    # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
    # and PackageReference isn't supported for native projects... [Sigh...]
    Write-Information "Restoring NuGet Packages"
    Invoke-NuGet restore 'src\Interop\Interop.sln' -PackagesDirectory $buildPaths.NuGetRepositoryPath -Verbosity quiet

    Write-Information "Building LibLLVM"
    Invoke-MSBuild -Targets Build -Project 'src\Interop\LibLLVM\LibLLVM.vcxproj' -Properties $msBuildProperties -LoggerArgs ($msbuildLoggerArgs + @("/bl:LibLLVM-build.binlog") )

    Write-Information "Building LllvmBindingsGenerator"
    Invoke-MSBuild -Targets Restore -Project 'src\Interop\LlvmBindingsGenerator\LlvmBindingsGenerator.csproj' -Properties $msBuildProperties -LoggerArgs ($msbuildLoggerArgs + @("/bl:LlvmBindingsGenerator.binlog") )
    Invoke-MSBuild -Targets Build -Project 'src\Interop\LlvmBindingsGenerator\LlvmBindingsGenerator.csproj' -Properties $msBuildProperties -LoggerArgs ($msbuildLoggerArgs + @("/bl:LlvmBindingsGenerator.binlog") )

    Write-Information "Generating P/Invoke Binding code"
    & "$($buildPaths.BuildOutputPath)\bin\LlvmBindingsGenerator\Release\net47\LlvmBindingsGenerator.exe" $buildPaths.LlvmLibsRoot (Join-Path $buildPaths.SrcRoot 'Interop\LibLLVM') (Join-Path $buildPaths.SrcRoot 'Interop\Llvm.NET.Interop') 

    Write-Information "Building Lllvm.NET.Interop"
    Invoke-MSBuild -Targets Build -Project 'src\Interop\Llvm.NET.Interop\Llvm.NET.Interop.csproj' -Properties $msBuildProperties -LoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET.Interop.binlog") )
}
finally
{
    popd
    $env:Path = $oldPath
}
