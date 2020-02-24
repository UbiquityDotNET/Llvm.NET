Param(
    [switch]$SkipPush
)

. .\buildutils.ps1

Initialize-BuildEnvironment

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
    throw "Pushing docs is only allowed when the origin remote is the official source release current remote is '$remoteUrl'"
}

if(!$env:docspush_access_token -and !$SkipPush)
{
    Write-Error "Missing docspush_access_token"
}

if(!$env:docspush_email)
{
    Write-Error "Missing docspush_email"
}

if(!$env:docspush_username)
{
    Write-Error "Missing docspush_username"
}

pushd .\BuildOutput\docs -ErrorAction Stop
try
{
    git config --global credential.helper store
    Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:docspush_access_token):x-oauth-basic@github.com`n"
    git config --global user.email "$env:docspush_email"
    git config --global user.name "$env:docspush_username"

    # use cmd /c to get message output from GH Actions that are suppressed when a secret is used.
    cmd /c echo Adding files to git
    git add -A
    git ls-files -o --exclude-standard | %{ git add $_}
    if(!$?)
    {
        throw "git add failed"
    }

    cmd /c echo Committing changes to git
    git commit -m "CI Docs Update"
    if(!$?)
    {
        throw "git commit failed"
    }

    if($SkipPush)
    {
        cmd /c echo Pushing changes to git
        git push
    }
}
catch
{
    cmd /c echo Error pushing docs:
    cmd /c echo ($_.Exception | out-string)
    throw
}
finally
{
    popd
}
