using module "PSModules/CommonBuild/CommonBuild.psd1"
using module "PSModules/RepoBuild/RepoBuild.psd1"

<#
.SYNOPSIS
    Shows docs using the docfx built-in server

.PARAMETER buildInfo
   BuildInfo to use for current repo build. The hashtable provided for this must include the
   `DocsOutputPath` value for the path to use. This is normally set by a call to `Initialize-Environment`

.PARAMETER DocsPathToHost
    Path of docs to host. Default is the local build but any legit path is accepted.
    This is useful when downloading docs build artifacts to ensure they are built correctly.
#>
Param(
    [hashtable]$buildInfo,
    [string]$DocsPathToHost
)

$docFXToolVersion = '2.78.4'

$InformationPreference = 'Continue'
$ErrorInformationPreference = 'Stop'

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    if (!$DocsPathToHost)
    {
        if (!$buildInfo)
        {
            $buildInfo = Initialize-BuildEnvironment
        }

        $DocsPathToHost = $buildInfo['DocsOutputPath']
    }

    if (!(Test-Path $DocsPathToHost -PathType Container))
    {
        throw "Path '$DocsPathToHost' does not exist or is not a directory"
    }

    # make sure the supported tool is installed.
    Invoke-External dotnet tool install --global docfx --version $docFXToolVersion | Out-Null
    Invoke-External docfx serve '--open-browser' $DocsPathToHost
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
