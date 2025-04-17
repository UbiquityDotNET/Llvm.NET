function Find-OnPath
{
<#
.SYNOPSIS
    Searches for an executable on the current environment search path

.PARAMETER exeName
    The executable to search for

.NOTES
    This is a simple wrapper around the Get-Command built-in with `-Type Application` to get
    the full path of an executable application. This allows invoking the application explicitly
    in a runtime neutral form. This applies exception handling to ensure that NOT-FOUND results
    in a $null instead of an exception/error message display. (The command might not exist and
    scripts must deal with that possibility)
#>
    [CmdletBinding()]
    [OutputType([string])]
    Param( [Parameter(Mandatory=$True,Position=0)][string]$appName)
    try
    {
        $retVal = (Get-command -Type Application $appName -ErrorAction Stop).Source
        if ($retVal -is [Array])
        {
            return $retVal[0]
        }
        return $retVal
    }
    catch
    {
        return $null
    }
}
