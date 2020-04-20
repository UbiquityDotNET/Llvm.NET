Param(
    [switch]$SkipPush
)

. .\buildutils.ps1
$buildInfo = Initialize-BuildEnvironment

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

    $msg = "CI Docs Update $(Get-BuildVersionTag)"
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
finally
{
    popd
}
