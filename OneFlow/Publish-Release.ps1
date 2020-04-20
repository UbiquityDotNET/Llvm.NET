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
if(!$?) {throw 'GIT tag command failed'}
git checkout develop
if(!$?) {throw 'GIT co develop command failed'}
git merge $releaseBranch
if(!$?) {throw 'GIT merge command failed'}
git push --tags origin develop
if(!$?) {throw 'GIT push --tags origin develop command failed'}
git branch -d $releaseBranch
if(!$?) {throw 'GIT delete local release branch command failed'}
git push origin --delete $releaseBranch
if(!$?) {throw 'GIT delete remote release branch command failed'}

# update master branch to point to the latest release for full releases
git checkout master
if(!$?) {throw 'GIT checkout master command failed'}
git merge --ff-only $tagName
if(!$?) {throw 'GIT merge --ff-only command failed'}
git push
if(!$?) {throw 'GIT push (master update) command failed'}
