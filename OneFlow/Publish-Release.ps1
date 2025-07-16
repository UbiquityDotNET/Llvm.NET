using module '../PSModules/CommonBuild/CommonBuild.psd1'
using module '../PsModules/RepoBuild/RepoBuild.psd1'

<#
.SYNOPSIS
    Publishes the current release as a new branch to the upstream repository

.DESCRIPTION
    Generally, this function will finalize the changes for the release and create a new "merge-back"
    branch to manage any conflicts to prevent commits "AFTER" the tag is applied to the origin repository.
    After this completes it is still required to create a PR for the resulting branch to the origin's "develop"
    branch to merge any changes in this branch - including the release tag.

    Completing the PR with a release tag should trigger the start the official build via a GitHub action
    or other such automated build processes. These, normally, also include publication of the resulting
    binaries as appropriate. This function only pushes the tag, the rest is up to the back-end configuration
    of the repository.

.NOTE
    For the gory details of this process see: https://www.endoflineblog.com/implementing-oneflow-on-github-bitbucket-and-gitlab
#>
[cmdletbinding(SupportsShouldProcess=$True, ConfirmImpact='High')]
Param()
$buildInfo = Initialize-BuildEnvironment

Write-Information "Build Version XML:"
Write-Information ((Get-ParsedBuildVersionXML -BuildInfo $buildInfo).GetEnumerator() | Sort -Property Name | Out-String)

# merging the tag to develop branch on the official repository triggers the official build and release of the NuGet Packages
$tagName = Get-BuildVersionTag $buildInfo
$officialRemoteName = Get-GitRemoteName $buildInfo official
$forkRemoteName = Get-GitRemoteName $buildInfo fork

$releaseBranch = "release/$tagName"
$officialReleaseBranch = "$officialRemoteName/$releaseBranch"

$mainBranchName = "master"
$officialMainBranch = "$officialRemoteName/$mainBranchName"

$mergeBackBranchName = "merge-back-$tagName"

Write-Information 'Fetching from official repository'
Invoke-External git fetch $officialRemoteName

if($PSCmdlet.ShouldProcess($officialReleaseBranch, "git switch -C $releaseBranch"))
{
    Write-Information "Switching to release branch [$officialReleaseBranch]"
    Invoke-External git switch '-C' $releaseBranch $officialReleaseBranch
}

if($PSCmdlet.ShouldProcess($tagName, "create signed tag"))
{
    Write-Information 'Creating a signed tag of this branch as the release'
    Invoke-External git tag -s $tagName '-m' "Official release: $tagName"

    Write-Information 'Verifying signature on tag'
    Invoke-External git tag -v $tagName
}

if($PSCmdlet.ShouldProcess($tagName, "git push $officialRemoteName tag"))
{
    Write-Information 'Pushing tag to official remote [Starts automated build release process]'
    Invoke-External git push $officialRemoteName tag $tagName
}

if($PSCmdlet.ShouldProcess($releasebranch, "git checkout -b $mergeBackBranchName"))
{
    Write-Information 'Creating local merge-back branch to merge changes associated with the release'
    # create a "merge-back" child branch to handle any updates/conflict resolutions when applying
    # the changes made in the release branch back to the develop branch.
    Invoke-External git checkout '-b' $mergeBackBranchName $releasebranch
}

if($PSCmdlet.ShouldProcess("$forkRemoteName $mergeBackBranchName", "git push"))
{
    Write-Information 'pushing merge-back branch to fork'
    Invoke-External git push $forkRemoteName $mergeBackBranchName
}

if($PSCmdlet.ShouldProcess("$tagName", "Reset main to point to release tag"))
{
    Write-Information 'Updating main to point to tagged release'
    Invoke-External git switch '-C' $mainBranchName $officialMainBranch
    Invoke-External git reset --hard $tagName
    Invoke-External git push --force $officialRemoteName $mainBranchName
}
