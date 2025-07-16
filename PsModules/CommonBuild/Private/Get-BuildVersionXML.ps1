function Get-BuildVersionXML
{
<#
.SYNOPSIS
    Retrieves the contents of the BuildVersion.XML file in the RepoRootPath

.DESCRIPTION
    Reads the contents of the BuildVersion.xml file and returns it as XML
    for additional processing.

.PARAMETER buildInfo
    Hashtable containing Information about the repository and build. This function
    requires the presence of a 'RepoRootPath' property to indicate the root of the
    repository containing the BuildVersion.xml file.

.PARAMETER xmlPath
    Path of the XML file to retrieve. This is mutually exclusive with the BuildInfo
    parameter, it is generally only used when developing these build scripts to
    explicitly retrieve the version from a path without needing the hashtable.
#>
    [OutputType([xml])]
    param (
        [Parameter(Mandatory=$true, ParameterSetName = 'BuildInfo', Position=0)][hashtable] $buildInfo,
        [Parameter(Mandatory=$true, ParameterSetName = 'Path', Position=0)][string] $xmlPath
    )

    if ($PsCmdlet.ParameterSetName -eq "BuildInfo")
    {
        return [xml](Get-Content (Join-Path $buildInfo['RepoRootPath'] 'BuildVersion.xml'))
    }
    else
    {
        return [xml](Get-Content $xmlPath)
    }
}
