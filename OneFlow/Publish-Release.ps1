. .\buildutils.ps1
$buildInfo = Initialize-BuildEnvironment

# Release tags must only be pushed from a repository with the official GitHub repository as the origin remote.
# This ensures that the links to source in the generated docs will have the correct URLs
# (e.g. docs pushed to the official repository MUST not have links to source in some private fork)
$remoteUrl = git ls-remote --get-url

Write-Information "Remote URL: $remoteUrl"

if($remoteUrl -ine "https://github.com/UbiquityDotNET/Llvm.NET.git")
{
    throw "Publishing a release tag is only allowed when the origin remote is the official source release current remote is '$remoteUrl'"
}

# pushing the tag to GitHub triggers the official build and release of the Nuget Packages
$tagName = Get-BuildVersionTag
$releaseBranch = "release/$tagName"
$currentBranch = git rev-parse --abbrev-ref HEAD
if( $releaseBranch -ne $currentBranch )
{
    throw "Current branch '$currentBranch' doesn't match the expected release branch from BuildVersion.xml, expected '$releaseBranch'"
}

$localCommitSha = git rev-parse --verify $releaseBranch
$remoteCommitSha = git rev-parse --verify "origin/$releaseBranch"
if( $localCommitSha -ne $remoteCommitSha )
{
    throw "Local HEAD is not the same as origin, these must be in sync so that only the tag itself is pushed"
}

Write-Information "tagging $tagname on $releasebranch"

git tag -a $tagname -m "Official release: $tagname"
git checkout develop
git merge $releaseBranch
git push --tags origin develop
git branch -d $releaseBranch
git push origin --delete $releaseBranch

# update master branch to point to the latest release for full releases
git checkout master
git merge --ff-only $tagName
git push
