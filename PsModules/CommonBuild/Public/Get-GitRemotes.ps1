function Get-GitRemotes
{
    param($opName = "push")
    $remoteLines = ((git remote -v) -split [System.Environment]::NewLine)
    $retVal = [System.Collections.ArrayList]@()
    foreach($remoteLine in $remoteLines)
    {
        if( $remoteLine -match '([^\s]+)\s+([^\s]+)\s+\(([^\s]+)\)')
        {
            if($matches[3] -eq $opName)
            {
                $hashTable =@{
                    Name = $matches[1]
                    Uri = $matches[2]
                    Op = $matches[3]
                }

                $retVal.Add($hashTable) | Out-Null
            }
        }
        else
        {
            throw "'$remoteLine' - does not match pattern for a valid git remote..."
        }
    }

    return $retVal
}

function Get-GitRemoteName
{
    param([hashtable]$bldInfo, [ValidateSet('official', 'fork')] $kind)

    if($kind -eq 'official')
    {
        Get-GitRemotes | ? {$_.Uri -eq $bldInfo['OfficialGitRemoteUrl']} | Select -ExpandProperty Name
    }
    else
    {
        Get-GitRemotes | ? {$_.Uri -ne $bldInfo['OfficialGitRemoteUrl']} | Select -ExpandProperty Name
    }
}
