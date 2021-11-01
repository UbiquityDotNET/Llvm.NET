<#
.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.

.PARAMETER SkipPush
    Performs all the steps up to but not including the actual final push to the parent repository. This is useful when debugging
    or otherwise diagnosing issues with the push process locally as it allows full access to the git repo and commit history, etc...
#>
Param(
    [switch]$AllowVsPreReleases,
    [switch]$FullInit,
    [switch]$SkipPush
)

. .\repo-buildutils.ps1
$buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit -AllowVsPreReleases:$AllowVsPreReleases

Write-Information "Preparing to PUSH updated docs to GitHub IO"

$canPush = $env:IsAutomatedBuild -and ($env:IsPullRequestBuild -ieq 'false')
if(!$canPush)
{
    Write-Information "Skipping Docs PUSH as this is not an official build"
    return;
}

# Docs must only be updated from a build with the official repository as the default remote.
# This ensures that the links to source in the generated docs will have the correct URLs
# (e.g. docs pushed to the official repository MUST not have links to source in some private fork)
$remoteUrl = git ls-remote --get-url

Write-Information "Remote URL: $remoteUrl"

if(!($remoteUrl -like "https://github.com/UbiquityDotNET/Llvm.NET*"))
{
    throw "Pushing docs is only allowed when the origin remote is the official source - current remote is '$remoteUrl'"
}

if(!$env:docspush_access_token -and !$SkipPush)
{
    Write-Error "Missing docspush_access_token" -ErrorAction Stop
}

if(!$env:docspush_email)
{
    Write-Error "Missing docspush_email" -ErrorAction Stop
}

if(!$env:docspush_username)
{
    Write-Error "Missing docspush_username" -ErrorAction Stop
}

pushd .\BuildOutput\docs -ErrorAction Stop
try
{
    if($env:docspush_access_token)
    {
        git config --local credential.helper store
        Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:docspush_access_token):x-oauth-basic@github.com`n"
    }

    git config --local user.email "$env:docspush_email"
    git config --local user.name "$env:docspush_username"

    Write-Information 'Adding files to git'
    git add -A
    git ls-files -o --exclude-standard | %{ git add $_}
    if(!$?)
    {
        throw "git add failed"
    }

    $msg = "CI Docs Update $(Get-BuildVersionTag $buildInfo)"
    Write-Information "Committing changes to git [$msg]"
    git commit -m"$msg"
    if(!$?)
    {
        throw "git commit failed"
    }

    if(!$SkipPush)
    {
        Write-Information 'Pushing changes to git'
        git push
    }
}
catch
{
    # everything from the official docs to the various articles in the blog-sphere says this isn't needed
    # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
    # information is retained and the error reported will include the correct source file and line number
    # data for the error. Without this, only the error message is retained and the location information is
    # Line 1, Column 1, of the outer most script file, which is, of course, completely useless.
    throw
}

finally
{
    popd
}
