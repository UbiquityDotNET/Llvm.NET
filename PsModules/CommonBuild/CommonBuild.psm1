# This script is a generalized script entry point for module support that is easily cloned for other modules
# and is independent of the module's functionality.

function Get-ExportedFunctionNames
{
    param(
        [System.IO.FileInfo[]] $publics = @( Get-ChildItem -Path (Join-path $PSScriptroot 'Public' '*.ps1') -ErrorAction SilentlyContinue )
    )

    $retVal = New-Object -TypeName 'System.Collections.ArrayList'
    foreach($pubScript in $publics)
    {
        $script = Get-Command $pubscript.FullName

        # use PS built-in parser to find all of the public functions
        $functionNames = $script.ScriptBlock.AST.FindAll(
          { $args[0] -is [Management.Automation.Language.FunctionDefinitionAst] },
          $false
        ).Name

        foreach($name in $functionNames)
        {
            if($name -like '*-*')
            {
                $retVal.Add($name) | Out-Null
            }
            else
            {
                Write-Information "found non-standard function mame '$name' in '$script'"
            }
        }
    }

    return $retVal
}

# Exported from the module but NOT from the PSD1 list. Instead, you should use module qualifed name
# to access this function. It is only useful when updating the set of public functions exported
# to generate the required 'FunctionsToExport' property of the PSD1 file.
function Get-FunctionsToExport
{
<#
.SYNOPSIS
    Gets the 'FunctionsToExport' node for the PSD1 file as a string
#>
    $bldr = New-Object -TypeName 'System.Text.StringBuilder' 'FunctionsToExport = @('
    $bldr.AppendLine() | Out-Null
    $exportedFunctionNames = Get-ExportedFunctionNames

    # Force inject the one known always present function
    # NOTE: PS needs BOTH the PSD and the calls to "Export-Member -Function"
    if(!$exportedNames -or $exportedNames.Length -eq 0)
    {
        $bldr.AppendLine("    'Get-FunctionsToExport'") | Out-Null
    }
    else
    {
        $bldr.AppendLine("    'Get-FunctionsToExport',") | Out-Null
    }

    for($i = 0; $i -lt $exportedFunctionNames.Length; ++$i)
    {
        $bldr.Append("    '$($exportedFunctionNames[$i])'") | Out-Null
        if($i -lt ($exportedFunctionNames.Length - 1))
        {
            $bldr.AppendLine(',') | Out-Null
        }
        else
        {
            $bldr.AppendLine() | Out-Null # Trailing comma is an error in PS; So, don't include it
        }
    }

    $bldr.Append(')') | Out-Null
    return $bldr.ToString()
}

# get public/Private function definition files
$Public = @( Get-ChildItem -Path (Join-path $PSScriptroot 'Public' '*.ps1') -ErrorAction SilentlyContinue )
$Private = @( Get-ChildItem -Path (Join-path $PSScriptroot 'Private' '*.ps1') -ErrorAction SilentlyContinue )

# Dot source the files so they are available in this script
foreach($import in @($Public + $Private))
{
    try
    {
        . $import.fullname
    }
    catch
    {
        Write-Error -Message "Failed to import function definition $($import.fullname): $_"
    }
}

# Explicitly calling Export-ModuleMember disables the default
# behavior of exporting ALL functions, even if they are marked
# with private scope! However, Listing the functions in the PSD
# FunctionsToExport member will ALSO export them AND make them
# available so PS can auto load this module when they are called
# thus, listing in the PSD file is the preferred method of exporting
# script functions.
#
# Export-ModuleMember Get-FunctionsToExport
#$exportedFunctionNames = Get-ExportedFunctionNames $Public
#foreach($name in $exportedFunctionNames )
#{
#    Export-ModuleMember $name
#}



