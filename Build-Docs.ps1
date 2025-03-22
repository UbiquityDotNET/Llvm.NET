<#
.SYNOPSIS
    Builds the docs for this repository

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this may be set to "Debug"

.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.

.PARAMETER NoClone
    Skip cloning of the docs repository. Useful for inner loop development where you only do the clone once when iterating on
    doc updates.

.PARAMETER ShowDocs
    Show the docs via `docfx serve --open-browser ...`. This option is useful for inner loop development of the docs as it allows
    opening a browser on the newly created docs for testing/checking the contents are produced correctly.
#>
Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$FullInit,
    [switch]$NoClone,
    [switch]$ShowDocs
)

function Invoke-DocFX
{
    docfx $args
    if(!$?)
    {
        throw "Invoke of dotnet failed [$LastExitCode]"
    }
}

function Get-FullBuildNumber
{
    # docfx no longer supports the docfxconsole and insists on doing everything manually
    # with the command line instead of within a project file... So do it the hard way...

    # First step is to generate a JSON file with the version information that comes
    # from the MSBUILD tasks in the `CSemVer.Build.Tasks` NUGET package. To do that
    # a no target project is used to capture the results of the versioning into a
    # JSON file, then that file is read in to get the values for PS so that a consistent
    # build number is used for the docs generation.

    # generate the CurrentVersionInfo.json file
    Invoke-DotNet build ./BuildVersion.proj | Out-Null

    # Read in the generated JSON for use in PS
    $versionInfo = Get-Content ./CurrentVersionInfo.json | ConvertFrom-Json -AsHashTable
    return "$($versionInfo['FullBuildNumber'])"
}

$docFXToolVersion = '2.78.3'

$InformationPreference = 'Continue'
$ErrorInformationPreference = 'Stop'

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    . ./repo-buildutils.ps1

    $buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit -AllowVsPreReleases:$AllowVsPreReleases
    $msBuildPropertyList = ConvertTo-PropertyList @{
        Configuration = $Configuration
    }

    # make sure the supported tool is installed.
    Invoke-DotNet tool install --global docfx --version $docFXToolVersion | Out-Null

    $docsOutputPath = $buildInfo['DocsOutputPath']
    Write-Verbose "Docs OutputPath: $docsOutputPath"

    # Clone docs output location so it is available as a destination for the Generated docs content
    # and the versioned docs links can function correctly for locally generated docs
    if(!$NoClone -and !(Test-Path (Join-Path $docsOutputPath '.git') -PathType Container))
    {
        if(Test-Path -PathType Container $docsOutputPath)
        {
            Write-Information "Cleaning $docsOutputPath"
            Remove-Item -Path $docsOutputPath -Recurse -Force
        }

        Write-Information "Cloning Docs repository"
        Invoke-Git clone $buildInfo['OfficialGitRemoteUrl'] -b gh-pages $docsOutputPath -q
    }

    # remove all contents from 'current' docs to ensure clean generated docs for this release
    $currentVersionDocsPath = Join-Path $docsOutputPath 'current'
    if(Test-Path -PathType Container $currentVersionDocsPath)
    {
        Write-Information 'Cleaning current version folder'
        Remove-Item -Path $currentVersionDocsPath -Recurse -Force
    }

    $fullBuildNumber = Get-FullBuildNumber

    push-location './docfx/current'
    try
    {
        Write-Information "Building current version library docs [FullBuildNumber=$fullBuildNumber]"
        Invoke-DocFX -m _buildVersion=$fullBuildNumber -o $docsOutputPath --warningsAsErrors

        Set-Location ..\index
        Write-Information "Building main landing page with version links [FullBuildNumber=$fullBuildNumber]"
        Invoke-DocFX -m _buildVersion="$fullBuildNumber"
    }
    finally
    {
        Pop-Location
    }

    if($ShowDocs)
    {
        Invoke-DocFx serve --open-browser $docsOutputPath
    }
}
catch
{
    # Everything from the official docs to the various articles in the blog-sphere says this isn't needed
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
