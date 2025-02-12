<#
.SYNOPSIS
    Script to Create a link from the locally generated LLVM libs for pre-publication testing/validation

.PARAMETER localLibs
    Location of the locally built libs to create a juntion to (I.e., the target of the junction).

.DESCRIPTION
    This script is used by local developers to establish a link to previously built local LLVM binaries.
    This is used prior to commiting/publication of those changes to allow testing/validating the generated binaries.
    Of particular interest for testing is the inclusion of the correct headers, libs and PDBs.
#>
[cmdletbinding()]
Param($localLibs)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    # Pull in the repo specific support and force a full initialization of all the environment
    # as this is a top level build command.
    . .\repo-buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit -AllowVsPreReleases:$false
    $llvmLibsRoot = $buildInfo['LlvmLibsRoot']

    New-Item -ItemType Junction -Path $llvmLibsRoot -Target $localLibs
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
    popd
    $env:Path = $oldPath
}

Write-Information "Done"
