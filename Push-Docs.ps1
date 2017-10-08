if($env:APPVEYOR_PULL_REQUEST_NUMBER)
{
    return;
}

# Docs must only be updated from a build with the official repo as the default remote.
# This ensures that the links to source in the generated docs will have the correct URLs
# (e.g. docs pushed to the official repo MUST not have links to source in some private fork)
$remoteUrl = git ls-remote --get-url
if($remoteUrl -ne "https://github.com/UbiquityDotNET/Llvm.NET.git")
{
    throw "Pushing docs is only allowed when the origin remote is the official source release"
}

if($env:APPVEYOR)
{
    git config --global credential.helper store
    Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:docspush_access_token):x-oauth-basic@github.com`n"
    git config --global user.email "$env:docspush_email"
    git config --global user.name "$env:docspush_username"
}

pushd .\BuildOutput\docs -ErrorAction Stop
try
{
    # prevent line ending conversions
    git config core.safecrlf false
    git config core.autocrlf input

    Write-Information "Adding files to git"
    git add *

    Write-Information "Commiting changes to git"
    git commit -m "CI Docs Update ($env:APPVEYOR_BUILD_VERSION)"

    Write-Information "pushing changes to git"
    git push -q
}
finally
{
    popd
}
