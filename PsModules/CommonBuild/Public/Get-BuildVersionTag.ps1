function Get-BuildVersionTag
{
<#
.SYNOPSIS
    Retrieves the git tag name to apply for this build.

.DESCRIPTION
    Reads the contents of the BuildVersion.xml file and generates a git
    release tag name for the current build.

    This is a standalone function instead of a property on the build
    information Hashtable so that it is always dynamically evaluated
    based on the current contents of the BuildVersion.XML file as that
    is generally updated when this is needed.

.PARAMETER buildInfo
    Hashtable containing Information about the repository and build. This function
    requires the presence of a 'RepoRootPath' property to indicate the root of the
    repository containing the BuildVersion.xml file.
#>
    [OutputType([string])]
    Param($buildInfo)

    # determine release tag from the build version XML file in the branch
    $buildVersionXml = (Get-BuildVersionXML $buildInfo)
    $buildVersionData = $buildVersionXml.BuildVersionData
    $preReleaseSuffix=""
    if($buildVersionData.PSObject.Properties['PreReleaseName'])
    {
        $preReleaseSuffix = "-$($buildVersionData.PreReleaseName)"
        if($buildVersionData.PSObject.Properties['PreReleaseNumber'])
        {
            $preReleaseSuffix += ".$($buildVersionData.PreReleaseNumber)"
            if($buildVersionData.PSObject.Properties['PreReleaseFix'])
            {
                $preReleaseSuffix += ".$($buildVersionData.PreReleaseFix)"
            }
        }
    }

    return "v$($buildVersionData.BuildMajor).$($buildVersionData.BuildMinor).$($buildVersionData.BuildPatch)$preReleaseSuffix"
}
