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

pushd .\BuildOutput\docs
try
{
    git commit -a -m "CI Docs Update ($env:APPVEYOR_BUILD_VERSION)"
    git push -q
}
finally
{
    popd
}
