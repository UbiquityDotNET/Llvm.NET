<#
.SYNOPSIS
    Builds the native code Extended LLVM C API DLL along with the interop .NET assembly for it

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
     native assemblies to place the in the correct "native" "runtimes" folder for NuGet to handle
     them.
#>
Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClean
)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -AllowVsPreReleases:$AllowVsPreReleases

    # <HACK>
    # for details of why this is needed, see src\PatchVsForLibClang\readme.md
    # min version with fix (VS 2019 16.9 preview 2)
    $minOfficialVersion = [System.Version]('16.9.30803.129')

    $vs = Find-VSInstance -PreRelease:$AllowVsPreReleases
    if($vs.InstallationVersion -lt $minOfficialVersion)
    {
        Write-Information "Building PatchVsForLibClang.exe"
        $msBuildProperties = @{ Configuration = 'Release'}
        $buildLogPath = Join-Path $buildInfo['BinLogsPath'] PatchVsForLibClang.binlog
        Invoke-MSBuild -Targets 'Restore;Build' -Project src\PatchVsForLibClang\PatchVsForLibClang.sln -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$buildLogPath") )

        pushd (Join-Path $buildInfo.BuildOutputPath 'bin\PatchVsForLibClang\Release\netcoreapp3.1')
        try
        {
            Write-Information "Patching VS CRT for parsing with LibClang..."
            .\PatchVsForLibClang.exe $vs.InstallationPath
            if ($LASTEXITCODE -ne 0) {
                throw "PatchVsForLibClang exited with code: $LASTEXITCODE"
            }
        }
        finally
        {
            popd
        }
    }
    else
    {
        Write-Information "$($vs.DisplayName) ($($vs.InstallationVersion)) already includes the official patch - skipping manual patch"
    }
    #</HACK>

    .\Move-LlvmBuild.ps1

    $msBuildProperties = @{ Configuration = $Configuration
                            LlvmVersion = $buildInfo['LlvmVersion']
                          }

    Write-Information "Building LllvmBindingsGenerator"
    $generatorBuildLogPath = Join-Path $buildInfo['BinLogsPath'] LlvmBindingsGenerator.binlog

    # manual restore needed so that the CppSharp libraries are available during the build phase
    # as CppSharp NuGet package is basically hostile to the newer SDK project format.
    Invoke-MSBuild -Targets 'Restore;Build' -Project 'src\Interop\LlvmBindingsGenerator\LlvmBindingsGenerator.csproj' -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$generatorBuildLogPath"))

    # At present CppSharp only supports the "desktop" framework, so limiting this to net47 for now
    # Hopefully they will support .NET Core soon, if not, the generation stage may need to move out
    # to a manual step with the results checked in.
    Write-Information "Generating P/Invoke Bindings"
    Write-Information "LlvmBindingsGenerator.exe $($buildInfo['LlvmLibsRoot']) $(Join-Path $buildInfo['SrcRootPath'] 'Interop\LibLLVM') $(Join-Path $buildInfo['SrcRootPath'] 'Interop\Ubiquity.NET.Llvm.Interop')"

    & "$($buildInfo['BuildOutputPath'])\bin\LlvmBindingsGenerator\Release\net47\LlvmBindingsGenerator.exe" $buildInfo['LlvmLibsRoot'] (Join-Path $buildInfo['SrcRootPath'] 'Interop\LibLLVM') (Join-Path $buildInfo['SrcRootPath'] 'Interop\Ubiquity.NET.Llvm.Interop')
    if($LASTEXITCODE -eq 0)
    {
        # now build the projects that consume generated output for the bindings

        # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
        # and PackageReference isn't supported for native projects... [Sigh...]
        Write-Information "Restoring LibLLVM"
        Invoke-NuGet restore 'src\Interop\LibLLVM\LibLLVM.vcxproj'

        Write-Information "Building LibLLVM"
        $libLLVMBinLogPath = Join-Path $buildInfo['BinLogsPath'] LibLLVM-Build.binlog
        Invoke-MSBuild -Targets 'Build' -Project 'src\Interop\LibLLVM\LibLLVM.vcxproj' -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$libLLVMBinLogPath") )

        Write-Information "Building Ubiquity.NET.Llvm.Interop"
        $interopSlnBinLog = Join-Path $buildInfo['BinLogsPath'] Interop.sln.binlog
        Invoke-MSBuild -Targets 'Restore;Build' -Project 'src\Interop\Interop.sln' -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$interopSlnBinLog") )
    }
    else
    {
        Write-Error "Generating LLVM Bindings failed"
    }
}
catch
{
    Write-Error $_.Exception.Message
}
finally
{
    popd
    $env:Path = $oldPath
}
