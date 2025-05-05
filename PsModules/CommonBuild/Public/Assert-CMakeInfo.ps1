function Assert-CmakeInfo([Version]$minVersion)
{
    $cmakeInfo = Invoke-External cmake -E capabilities | ConvertFrom-Json
    if(!$cmakeInfo)
    {
        throw "CMake version not supported. 'cmake -E capabilities' returned nothing"
    }

    $cmakeVer = [Version]::new($cmakeInfo.version.major,$cmakeInfo.version.minor,$cmakeInfo.version.patch)
    if( $cmakeVer -lt $minVersion )
    {
        throw "CMake version not supported. Found: $cmakeVer; Require >= $($minVersion)"
    }
}
