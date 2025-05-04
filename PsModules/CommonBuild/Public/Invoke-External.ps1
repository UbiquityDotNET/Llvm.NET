function Invoke-External($cmd)
{
<#
.SYNOPSIS
    Invokes an external command with arguments converting any failure (non-zero return) into an exception

.PARAMETER cmd
    The external command to run. This may be a full path, relative path or just the cmd name (assuming it is
    executable from the current environment search paths). Any additional arguments are passed to the cmd
    as-is.

.DESCRIPTION
    This provides two core features:
    1) Platform neutral execution of an external command that might conflict with a built-in alias.
    2) Detection of success for an external command (as a non-zero return code) and throws an exception
        - This is the normal behaviour for command line applications. If the app returns a non-zero value
          as a success code, then this script may not be used as it would consider that a failure.
#>
    $cmdPath = $cmd
    if(!(Test-Path -Type Leaf $cmd))
    {
        $cmdPath = Find-OnPath $cmd
    }

    if(!$cmdPath -or !(Test-Path -Type Leaf $cmdPath))
    {
        throw "External command '$cmd' not found. (Not available on environment's current search path)"
    }

    if($args)
    {
        & $cmdPath $args
        if(!$?)
        {
            throw "'$cmdPath $args' command failed ($LASTEXITCODE)"
        }
    }
    else
    {
        & $cmdPath
        if(!$?)
        {
            throw "'$cmdPath' command failed ($LASTEXITCODE)"
        }
    }
}
