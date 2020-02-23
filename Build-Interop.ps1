# This script is unfortunately necessary due to several factors:
# 1. SDK projects cannot reference VCXPROJ files correctly since they are multi-targeting
#    and VCXproj projects are not
# 2. CppSharp NUGET package is basically hostile to SDK projects and would otherwise require
#    the use of packages.config
# 3. packages.config NUGET is hostile to shared version control as it insists on placing full
#    paths to the NuGet dependencies and build artifacts into the project file. (e.g. adding a
#    NuGet dependency modifies the project file to inject hard coded FULL paths guaranteed to
#    fail on any version controlled project when built on any other machine)
# 4. Common resolution to #3 is to use a Nuget.Config to re-direct packages to a well known
#    location relative to the build root. However, that doesn't work since CppSharp and its
#    weird copy output folder dependency has hard coded assumptions of a "packages" folder
#    at the solution level.
# 5. Packing the final NugetPackage needs the output of the native code project AND that of
#    the managed interop library. But as stated in #1 there can't be a dependency so something
#    has to manage the build ordering independent of the multi-targeting.
#
# The solution to all of the above is a combination of elements
# 1. The LlvmBindingsGenerator is an SDK project. This effectively disables the gross copy
#    output hack and allows the use of project references. (Though on it's own is not enough
#    as the CppSharp assemblies aren't available to the build)
# 2. The LlvmBindingsGenerator.csproj has a custom "None" ItemGroup that copies the CppSharp
#    libs to the output directory so that the build can complete. (Though it requires a restore
#    pass to work so that the items are copied during restore and available at build)
# 3. This script to control the ordering of the build so that the native code is built, then the
#    interop lib is restored and finally the interop lib is built with multi-targeting.
# 4. The interop assembly project includes the NuGet packing with "content" references to the
#    native assemblies to place the in the correct "native" "runtimes" folder for NuGet to handle
#    them.
Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClean,
    [Parameter(ParameterSetName='FullBuild')]
    $BuildInfo
)

. .\buildutils.ps1
Initialize-BuildEnvironment

pushd $PSScriptRoot
$oldPath = $env:Path
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"
try
{
    $buildPaths = Get-BuildPaths $PSScriptRoot

    md $buildPaths.NuGetOutputPath -ErrorAction SilentlyContinue | Out-Null

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

    Write-Information "Building LllvmBindingsGenerator"
    $generatorBuildLogPath = Join-Path $buildPaths.BuildOutputPath LlvmBindingsGenerator.binlog
    # manual restore needed so that the CppSharp libraries are available during the build phase as CppSharp NuGet package
    # is basically hostile to the newer SDK project format.
    Invoke-MSBuild -Targets 'Restore;Build' -Project 'src\Interop\LlvmBindingsGenerator\LlvmBindingsGenerator.csproj' -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:$generatorBuildLogPath"))

    Write-Information "Generating P/Invoke Bindings"
    & "$($buildPaths.BuildOutputPath)\bin\LlvmBindingsGenerator\Release\net47\LlvmBindingsGenerator.exe" $buildPaths.LlvmLibsRoot (Join-Path $buildPaths.SrcRoot 'Interop\LibLLVM') (Join-Path $buildPaths.SrcRoot 'Interop\Llvm.NET.Interop')
    if($LASTEXITCODE -eq 0)
    {
        # now build the projects that consume generated output for the bindings

        # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
        # and PackageReference isn't supported for native projects... [Sigh...]
        Write-Information "Restoring LibLLVM"
        Invoke-NuGet restore 'src\Interop\LibLLVM\LibLLVM.vcxproj'

        Write-Information "Building LibLLVM"
        Invoke-MSBuild -Targets 'Build' -Project 'src\Interop\LibLLVM\LibLLVM.vcxproj' -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:LibLLVM.binlog") )

        Write-Information "Building Lllvm.NET.Interop"
        $interopRestoreBinLogPath = Join-Path $buildPaths.BinLogsPath Llvm.NET.Interop-restore.binlog
        $interopBinLog = Join-Path $buildPaths.BinLogsPath Llvm.NET.Interop.binlog
        Invoke-MSBuild -Targets 'Restore' -Project 'src\Interop\Llvm.NET.Interop\Llvm.NET.Interop.csproj' -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:$InteropRestoreBinLogPath") )
        Invoke-MSBuild -Targets 'Build' -Project 'src\Interop\Llvm.NET.Interop\Llvm.NET.Interop.csproj' -Properties $msBuildProperties -LoggerArgs ($BuildInfo.MsBuildLoggerArgs + @("/bl:$interopBinLog") )
    }
    else
    {
        Write-Error "Generating LLVM Bindings failed"
    }
}
finally
{
    popd
    $env:Path = $oldPath
}
