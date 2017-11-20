# Unfortunately, just getting DOCFX to successfully build requires a lot of undocumented
# workarounds for several bugs. Most of the problems come from it's use of MSBuild and
# Roslyn causing build conflicts with DotNet and VS build. It is beyond me why a doc
# generator tool needs to build a project, let alone why it needs to include the Roslyn
# Compiler too... [sigh]...
#
# This script will manually install the packages needed to run DOCFX and the memberpage
# plug-in used for the documentation. The script then creates a VCVARS environment to run
# DOCFX to work around the dependency and conflict problems with DOCFX and CoreCLR projects.

. .\buildutils.ps1

#--- Start of main script
pushd $PSScriptRoot
$oldPath = $env:Path
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"
try
{
    $InformationPreference='Continue'

    $buildPaths = Get-BuildPaths $PSScriptRoot

    Write-Information "Build Paths:"
    Write-Information ($buildPaths | Format-Table | Out-String)

    if( Test-Path -PathType Container $buildPaths.BuildOutputPath )
    {
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $buildPaths.BuildOutputPath
    }
    
    if( Test-Path -PathType Container src\Llvm.NET\obj\xdoc )
    {
        rd -Recurse -Force -Path src\Llvm.NET\obj\xdoc
    }

    rm docfx\api\*.yml
    rm docfx\api\.manifest

    if( !( Test-Path -PathType Container tools ) )
    {
        md tools | out-null
    }

    if( !( Test-Path -PathType Container $buildPaths.BuildOutputPath ) )
    {
        md BuildOutput | out-null
    }

    Write-Information "Initializing VsEnv"
    Initialize-VCVars

    if(!(Test-Path 'tools\memberpage\content' -PathType Container))
    {
        Write-Information "Fetching memberpage plugin and content"
        Invoke-NuGet install memberpage -Verbosity quiet -ExcludeVersion -OutputDirectory tools
    }

    $docfxPath = Find-OnPath docfx.exe -ErrorAction Continue
    if(!$docfxPath)
    {
        $docfxPath = Join-Path $PSScriptRoot 'tools'
        Invoke-NuGet install docfx.console -Verbosity quiet -ExcludeVersion -OutputDirectory $docfxPath
        $docfxPath = Join-Path $docfxPath 'docfx.console\tools'
        if( ( $env:Path -split ';' ) -inotcontains $docfxPath )
        {
            $env:Path = "$env:Path;$docfxPath"
        }
    }

    Invoke-Nuget Install msdn.4.5.2 -ExcludeVersion -PreRelease -OutputDirectory tools -Verbosity quiet

    if( !(Test-Path ".\BuildOutput\docs\.git" -PathType Container))
    {
        Write-Information "Cloning Docs repo"
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

    # DOCFX is inconsistent on relative paths in the docfx.json file
    # (i.e. Metadata[x].src[y].src is relative to the docfx.json file
    # but Metadata[x].dest is relative to the current directory.) So,
    # workaround the inconsistency by switching to the directory with
    # the DOCFX file so the difference is not relevant.
    pushd docfx
    try
    {
        Write-Information "Generating docs"
        docfx
    }
    finally
    {
        popd
    }
}
finally
{
    popd
    $env:Path = $oldPath
}
