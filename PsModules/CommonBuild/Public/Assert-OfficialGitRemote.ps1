function Assert-OfficialGitRemote
{
<#
.SYNOPSIS
    Verifies the current git remote is the official repository

.PARAMETER OfficialUrl
    URL of the official remote to Verify

.PARAMETER Activity
    String describing the activity. This is used in the exception message stating
    that the activity is only valid with the correct remote URL

.DESCRIPTION
    Some operations like, Release tags, and docs updates must only be pushed from a repository with the
    official GitHub repository as the origin remote. This, among other things, ensures that the links
    to source in the generated docs will have the correct URLs (e.g. docs pushed to the official repository
    MUST not have links to source in some private fork). This function is used, primarily in OneFlow release
    management scripts to ensure operations are done using the correct remote.
#>
    Param(
        [Parameter(Mandatory=$True)][string]$OfficialUrl,
        [Parameter(Mandatory=$True)][string]$Activity
    )

    $remoteUrl = Invoke-External git ls-remote --get-url
    if($remoteUrl -ine $OfficialUrl)
    {
        throw "$Activity is only allowed when the origin remote is the official repository - current remote is '$remoteUrl'"
    }
}
