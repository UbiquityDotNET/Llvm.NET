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
#>
    [OutputType([xml])]
    param ($buildInfo)

    return [xml](Get-Content (Join-Path $buildInfo['RepoRootPath'] 'BuildVersion.xml'))
}
