# Supports publishing to NUGET gallery
# This support, like the GitHub NuGet gallery itself, is a work in progress
# and should not be relied upon until it is finalized.
. .\buildutils.ps1

# NuGet packages must only be pushed from a build with the official repository as the default remote.
# This ensures that the published packages and PDBS etc... all work together
$remoteUrl = git ls-remote --get-url
if($remoteUrl -ne "https://github.com/UbiquityDotNET/Llvm.NET.git")
{
    throw "Pushing NUGET packages is only allowed when the origin remote is the official source release"
}

# Only publish NUGET on an official release build (it may still be a pre-release version but isn't a CI or buddy build)
if($env:CI && !($env:APPVEYOR_PULL_REQUEST_NUMBER) -and ($env:APPVEYOR_REPO_TAG))
{
    $nugetFeedName ='GitHub NuGet registry'
    $nugetFeedURL = 'https://nuget.pkg.github/UbiquityDotNET/index.json'
    Invoke-Nuget source Add '-Name' $gitHubNugetReg '-Source' $nugetFeedURL '-UserName' "$env:docspush_username" '-Password' "$env:docspush_access_token"

    pushd .\BuildOutput\Nuget -ErrorAction Stop
    try
    {
        Invoke-Nuget push '-Source' $gitHubNugetReg
    }
    finally
    {
        popd
    }
}
