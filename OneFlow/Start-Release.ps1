<#
.SYNOPSIS
    Creates a new public release branch the current branch state as an official release tag

.PARAMETER commit
    [Optional] Indicates the specific commit to create the branch from. Default is the head
    of the branch.

.DESCRIPTION
    This function creates and publishes a Release branch
#>

Param([switch]$TagOnly, [string]$commit = "")
$repoRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, ".."))
. (join-path $repoRoot repo-buildutils.ps1)
$buildInfo = Initialize-BuildEnvironment

# Create script scoped alias for git that throws a PowerShell exception if the command fails
Set-Alias git Invoke-git -scope Script -option Private

# create new local branch for the release
$branchName = "release/$(Get-BuildVersionTag $buildInfo -ReleaseNameOnly)"
git checkout -b $branchName $commit

# Push to origin so it is clear to all a release is in process
git push origin $branchName

# push to fork so that changes go through normal PR process
git push -u $branchName
