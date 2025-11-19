function Show-FullBuildInfo
{
<#
.SYNOPSIS
    Displays details of the build information and environment to the information and verbose streams

.PARAMETER buildInfo
    The build information Hashtable for the build. This normally contains the standard and repo specific
    properties so that the full details are available in logs.

.DESCRIPTION
    This function displays all the properties of the build info to the information stream. Additionally,
    details of the current PATH, the .NET SDKs and runtimes installed is logged to the Verbose stream.
#>
    Param($buildInfo)

    Write-Information 'Build Info:'
    Write-Information ($buildInfo | Format-Table | Out-String)

    Write-Information "BuildKind: $($buildInfo['CurrentBuildKind'])"
    Write-Information "CiBuildName: $env:CiBuildName"
    Write-Information "env: Is*"
    Write-Information (dir env:Is* | Format-Table -Property Name, value | Out-String)

    # This sort of detail is really only needed when solving problems with a runner
    Write-Verbose 'PATH:'
    $($env:Path -split ';') | %{ Write-Verbose $_ }

    Write-Verbose ".NET Runtimes:"
    Write-Verbose (dotnet --list-runtimes | Out-String)

    Write-Verbose ".NET SDKs:"
    Write-Verbose (dotnet --list-sdks | Out-String)
}
