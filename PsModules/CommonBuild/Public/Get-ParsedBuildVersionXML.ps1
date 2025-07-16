function Get-ParsedBuildVersionXML
{
<#
.SYNOPSIS
    Retrieves the contents of the BuildVersion.XML file in the RepoRootPath and parses it to a hashtable

.DESCRIPTION
    Reads the contents of the BuildVersion.xml file and returns it as parsed hashtable
    where each attribute of the root element is a key in the table.

.PARAMETER buildInfo
    Hashtable containing Information about the repository and build. This function
    requires the presence of a 'RepoRootPath' property to indicate the root of the
    repository containing the BuildVersion.xml file.

.PARAMETER xmlPath
    Path of the XML file to retrieve. This is mutually exclusive with the BuildInfo
    parameter, it is generally only used when developing these build scripts to
    explicitly retrieve the version from a path without needing the hashtable.
#>
    [OutputType([hashtable])]
    param (
        [Parameter(Mandatory=$true, ParameterSetName = 'BuildInfo', Position=0)][hashtable] $buildInfo,
        [Parameter(Mandatory=$true, ParameterSetName = 'Path', Position=0)][string] $xmlPath
    )

    if ($PsCmdlet.ParameterSetName -eq "BuildInfo")
    {
        $xml = Get-BuildVersionXML -BuildInfo $buildInfo
    }
    else
    {
        $xml = Get-BuildVersionXML -XmlPath $xmlPath
    }

    return ($xml.BuildVersionData | select-xml "@*" | foreach-object {$out = @{}} {$out[$_.Node.Name] = $_.Node.Value} {$out})
}
