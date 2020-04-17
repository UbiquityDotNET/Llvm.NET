param($featurename)

. .\buildutils.ps1
$buildInfo = Initialize-BuildEnvironment

# feature branch merges must only be pushed from a repository with the official GitHub repository as the origin remote.
$remoteUrl = git ls-remote --get-url
if( $remoteUrl -ine "https://github.com/UbiquityDotNET/Llvm.NET.git" )
{
    throw "Publishing a release tag is only allowed when the origin remote is the official source release current remote is '$remoteUrl'"
}

$featureBranchName = "feature/$featureName"
git checkout $featureBranchName
git pull
git rebase -i develop
git checkout develop
git merge --ff-only $featureBranchName
git push origin develop
git branch -d $featureBranchName
git push origin --delete $featureBranchName
