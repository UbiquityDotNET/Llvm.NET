function ConvertTo-NormalizedPath([string]$path )
{
<#
.SYNOPSIS
    Converts a potentially relative folder path to an absolute one with a trailing delimiter

.PARAMETER path
    Path to convert

.NOTES
    The delimiters in the path are converted to the native system preferred form during conversion
#>
    if(![System.IO.Path]::IsPathRooted($path))
    {
        $path = [System.IO.Path]::Combine((Get-Location).Path,$path)
    }

    $path = [System.IO.Path]::GetFullPath($path)
    if( !$path.EndsWith([System.IO.Path]::DirectorySeparatorChar) -and !$path.EndsWith([System.IO.Path]::AltDirectorySeparatorChar))
    {
        $path = $path + [System.IO.Path]::DirectorySeparatorChar
    }
    return $path
}
