function Assert-CMakeList([Parameter(Mandatory=$true)][string] $root)
{
    $cmakeListPath = Join-Path $root CMakeLists.txt
    if( !( Test-Path -PathType Leaf $cmakeListPath ) )
    {
        throw "'CMakeLists.txt' is missing, '$root' does not appear to be a valid CMAKE source directory"
    }
}
