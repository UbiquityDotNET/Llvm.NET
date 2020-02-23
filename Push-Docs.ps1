. .\buildutils.ps1

Initialize-BuildEnvironment

Write-Information "Preparing to PUSH updated docs to GitHub IO"

$canPush = $IsAutomatedBuild -and !$IsPullRequestBuild
if(!$canPush)
{
    Write-Information "Skipping Docs PUSH as this is not an official build"
    return;
}

# Docs must only be updated from a build with the official repository as the default remote.
# This ensures that the links to source in the generated docs will have the correct URLs
# (e.g. docs pushed to the official repository MUST not have links to source in some private fork)
$remoteUrl = git ls-remote --get-url
if(!($remoteUrl -like 'https://github.com/UbiquityDotNET/Llvm.NET*'))
{
    throw "Pushing docs is only allowed when the origin remote is the official source release current remote is '$remoteUrl'"
}

pushd .\BuildOutput\docs -ErrorAction Stop
try
{
    # Best effort, on git commands as they can return non-zero even if nothing is wrong.
    $ErrorActionPreference = 'Continue'

    Write-Information "pushing changes to git"
    git push
}
finally
{
    popd
}
