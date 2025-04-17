function Get-DefaultBuildPaths()
{
<#
.SYNOPSIS
    Gets the default set of paths for a build

.DESCRIPTION
    This function initializes a hash table with the default paths for a build. This
    allows for standardization of build output locations etc... across builds and repositories
    in the organization. The values set are as follows:

    | Name                | Description                          |
    |---------------------|--------------------------------------|
    | RepoRootPath        | Root of the repository for the build |
    | BuildOutputPath     | Base directory for all build output during the build |
    | NuGetRepositoryPath | NuGet 'packages' directory for C++ projects using packages.config |
    | NuGetOutputPath     | Location where NuGet packages created during the build are placed |
    | SrcRootPath         | Root of the source code for this repository |
    | DocsOutputPath      | Root path for the generated documentation for the project |
    | BinLogsPath         | Path to where the binlogs are generated for PR builds to allow diagnosing failures in the automated builds |
    | TestResultsPath     | Path to where test results are placed. |
    | DownloadsPath       | Location where any downloaded files, used by the build are placed |
    | ToolsPath           | Location of any executable tools downloaded for the build (Typically expanded from a compressed download) |
#>
    [OutputType([hashtable])]
    param(
        [ValidateNotNullorEmpty()]
        [string]$repoRoot
    )

    $buildOutputPath = ConvertTo-NormalizedPath (Join-Path $repoRoot 'BuildOutput')
    $buildPaths = @{
        RepoRootPath = $repoRoot
        BuildOutputPath = $buildOutputPath
        NuGetRepositoryPath = Join-Path $buildOutputPath 'packages'
        NuGetOutputPath = Join-Path $buildOutputPath 'NuGet'
        SrcRootPath = Join-Path $repoRoot 'src'
        DocsRepoPath = Join-Path $buildOutputPath 'docs'
        DocsOutputPath = Join-Path $buildOutputPath 'docs'
        BinLogsPath = Join-Path $buildOutputPath 'BinLogs'
        TestResultsPath = Join-Path $buildOutputPath 'Test-Results'
        DownloadsPath = Join-Path $repoRoot 'Downloads'
        ToolsPath = Join-Path $repoRoot 'Tools'
    }

    $buildPaths.GetEnumerator() | %{ Ensure-PathExists $_.Value }
    return $buildPaths
}
