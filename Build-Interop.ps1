<#
.SYNOPSIS
    Builds the native code Extended LLVM C API DLL along with the interop .NET assembly for it

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this may be set to "Debug"

.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.

.NOTE
This script is unfortunately necessary due to several factors:
  1. SDK projects cannot reference VCXPROJ files correctly since they are multi-targeting
     and VCXproj projects are not
  2. CppSharp NUGET package is basically hostile to SDK projects and would otherwise require
     the use of packages.config
  3. packages.config NUGET is hostile to shared version control as it insists on placing full
     paths to the NuGet dependencies and build artifacts into the project file. (e.g. adding a
     NuGet dependency modifies the project file to inject hard coded FULL paths guaranteed to
     fail on any version controlled project when built on any other machine)
  4. Common resolution to #3 is to use a Nuget.Config to re-direct packages to a well known
     location relative to the build root. However, that doesn't work since CppSharp and its
     weird copy output folder dependency has hard coded assumptions of a "packages" folder
     at the solution level.
  5. Packing the final NugetPackage needs the output of the native code project AND that of
     the managed interop library. But as stated in #1 there can't be a dependency so something
     has to manage the build ordering independent of the multi-targeting.

  The solution to all of the above is a combination of elements
  1. The LlvmBindingsGenerator is an SDK project. This effectively disables the gross copy
     output hack and allows the use of project references. (Though on it's own is not enough
     as the CppSharp assemblies aren't available to the build)
  2. The LlvmBindingsGenerator.csproj has a custom "None" ItemGroup that copies the CppSharp
     libs to the output directory so that the build can complete. (Though it requires a restore
     pass to work so that the items are copied during restore and available at build)
  3. This script to control the ordering of the build so that the native code is built, then the
     interop lib is restored and finally the interop lib is built with multi-targeting.
  4. The interop assembly project includes the NuGet packing with "content" references to the
     native assemblies to place the binaries in the correct "native" "runtimes" folder for NuGet
     to handle them.
#>
Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$FullInit
)

Push-location $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\repo-buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit -AllowVsPreReleases:$AllowVsPreReleases

    # Download and unpack the LLVM libs if not already present, this doesn't use NuGet as the NuGet compression
    # is insufficient to keep the size reasonable enough to support posting to public galleries. Additionally, the
    # support for native lib projects in NuGet is tenuous at best. Due to various compiler version dependencies
    # and incompatibilities libs are generally not something published in a package. However, since the build time
    # for the libraries exceeds the time allowed for most hosted build services these must be pre-built for the
    # automated builds.
    Install-LlvmLibs $buildInfo

    $msBuildProperties = @{ Configuration = $Configuration
                            LlvmVersion = $buildInfo['LlvmVersion']
                          }
    $msbuildPropertyList = ConvertTo-PropertyList $msBuildProperties

    Write-Information "Building LllvmBindingsGenerator"

    dotnet build 'src\Interop\LlvmBindingsGenerator\LlvmBindingsGenerator.csproj' -p:$msbuildPropertyList

    Write-Information "Generating P/Invoke Bindings"
    Write-Information "LlvmBindingsGenerator.exe $($buildInfo['LlvmLibsRoot']) $(Join-Path $buildInfo['SrcRootPath'] 'Interop\LibLLVM') $(Join-Path $buildInfo['SrcRootPath'] 'Interop\Ubiquity.NET.Llvm.Interop')"

    dotnet "$($buildInfo['BuildOutputPath'])\bin\LlvmBindingsGenerator\$Configuration\net8.0\LlvmBindingsGenerator.dll" $buildInfo['LlvmLibsRoot'] (Join-Path $buildInfo['SrcRootPath'] 'Interop\LibLLVM') (Join-Path $buildInfo['SrcRootPath'] 'Interop\Ubiquity.NET.Llvm.Interop')
    if($LASTEXITCODE -eq 0)
    {
        # now build the projects that consume generated output for the bindings

        # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
        # and PackageReference isn't supported for native projects... [Sigh...]

        # TODO: Convert C++ code to CMAKE, this means leveraging CSmeVer build task as a standalone tool so that the version Information
        # is available to the scripts to provide to the build. Also means dealing with PDBgit in some fashion to establish symbols
        # correctly to the source, For now leave it on the legacy direct calls to MSBUILD as dotnet msbuild can't find any of the
        # C++ support - Seems it's using a different set of msbuild props/targets files...
        Write-Information "Restoring LibLLVM"
        Invoke-NuGet restore 'src\Interop\LibLLVM\LibLLVM.vcxproj'

        Write-Information "Building LibLLVM"
        $libLLVMBinLogPath = Join-Path $buildInfo['BinLogsPath'] LibLLVM-Build.binlog
        Invoke-MSBuild -Targets 'Build' -Project 'src\Interop\LibLLVM\LibLLVM.vcxproj' -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$libLLVMBinLogPath") )

        Write-Information "Building Ubiquity.NET.Llvm.Interop"
        dotnet build 'src\Interop\Interop.sln' -p:$msbuildPropertyList
    }
    else
    {
        Write-Error "Generating LLVM Bindings failed" -ErrorAction Stop
    }
}
catch
{
    # everything from the official docs to the various articles in the blog-sphere says this isn't needed
    # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
    # information is retained and the error reported will include the correct source file and line number
    # data for the error. Without this, only the error message is retained and the location information is
    # Line 1, Column 1, of the outer most script file, which is, of course, completely useless.
    throw
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}
