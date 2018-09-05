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

    Write-Information "Build Paths:"
    Write-Information ($buildPaths | Format-Table | Out-String)

    if( (Test-Path -PathType Container $buildPaths.BuildOutputPath) -and !$NoClean )
    {
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $buildPaths.BuildOutputPath
    }

    md $buildPaths.NuGetOutputPath -ErrorAction SilentlyContinue| Out-Null

    $BuildInfo = Get-BuildInformation $buildPaths
    if($env:APPVEYOR)
    {
        Update-AppVeyorBuild -Version $BuildInfo.FullBuildNumber
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

    Write-Information "Build Parameters:"
    Write-Information ($BuildInfo | Format-Table | Out-String)

    # Download and unpack the LLVM libs if not already present, this doesn't use NuGet as the NuGet compression
    # is insufficient to keep the size reasonable enough to support posting to public galleries. Additionally, the
    # support for native lib projects in NuGet is tenuous at best. Due to various compiler version dependencies
    # and incompatibilities libs are generally not something published in a package. However, since the build time
    # for the libraries exceeds the time allowed for most hosted build services these must be pre-built for the
    # automated APPVEYOR builds.
    Install-LlvmLibs $buildPaths.LlvmLibsRoot

    # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
    # and PackageReference isn't supported for native projects
    Write-Information "Restoring NuGet Packages for LibLLVM.vcxproj"
    Invoke-NuGet restore src\LibLLVM\LibLLVM.vcxproj -PackagesDirectory $buildPaths.NuGetRepositoryPath -Verbosity quiet

    Write-Information "Building LibLLVM"
    Invoke-MSBuild -Targets Build -Project src\LibLLVM\LibLLVM.vcxproj -Properties $msBuildProperties -LoggerArgs ($msbuildLoggerArgs + @("/bl:LibLLVM-build.binlog") )

    Write-Information "Packing LibLLVM"
    Invoke-MSBuild -Targets Pack -Project src\LibLLVM\LibLLVM.vcxproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:LibLLVM-pack.binlog") )

    Write-Information "Restoring NuGet Packages for Llvm.NET"
    Invoke-MSBuild -Targets Restore -Project src\Llvm.NET.sln -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET-restore.binlog") )
    if( !(Test-Path (Join-Path $buildPaths.DocsOutput '.git') -PathType Container))
    {
        Write-Information "Cloning Docs repository"
        pushd BuildOutput -ErrorAction Stop
        try
        {
            git clone https://github.com/UbiquityDotNET/Llvm.NET.git -b gh-pages docs -q
        }
        finally
        {
            popd
        }
    }

    Write-Information "Restoring Docs Project"
    Invoke-MSBuild -Targets Restore -Project docfx\Llvm.NET.DocFX.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET-docfx-restore.binlog") )

    Write-Information "Building Docs"
    Invoke-MSBuild -Targets Build -Project docfx\Llvm.NET.DocFX.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET-docfx-build.binlog") )

    Write-Information "Building Llvm.NET"
    Invoke-MSBuild -Targets Build -Project src\Llvm.NET.sln -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Llvm.NET-build.binlog") )
    
    if( $env:APPVEYOR_PULL_REQUEST_NUMBER )
    {
        foreach( $item in Get-ChildItem *.binlog )
        {
            Push-AppveyorArtifact $item.FullName
        }
    }
}
finally
{
    popd
    $env:Path = $oldPath
}
