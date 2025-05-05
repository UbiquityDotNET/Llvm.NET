function ConvertTo-PropertyList([hashtable]$hashTable)
{
<#
.SYNOPSIS
    Converts a hash table into a semi-colon delimited property list
#>
    return ( $hashTable.GetEnumerator() | %{ @{$true=$_.Key;$false= $_.Key + "=" + $_.Value }[[string]::IsNullOrEmpty($_.Value) ] } ) -join ';'
}
