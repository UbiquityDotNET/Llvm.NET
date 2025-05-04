enum BuildKind
{
    LocalBuild
    PullRequestBuild
    CiBuild
    ReleaseBuild
}

function Get-CurrentBuildKind
{
<#
.SYNOPSIS
    Determines the kind of build for the current environment

.DESCRIPTION
    This function retrieves environment values set by various automated builds
    to determine the kind of build the environment is for. The return is one of
    the [BuildKind] enumeration values:

    | Name             | Description |
    |------------------|-------------|
    | LocalBuild       | This is a local developer build (e.g. not an automated build)
    | PullRequestBuild | This is a build from a PullRequest with untrusted changes, so build should limit the steps appropriately |
    | CiBuild          | This build is from a Continuous Integration (CI) process, usually after a PR is accepted and merged to the branch |
    | ReleaseBuild     | This is an official release build, the output is ready for publication (Automated builds may use this to automatically publish) |
#>
    [OutputType([BuildKind])]
    param()

    $currentBuildKind = [BuildKind]::LocalBuild

    # IsAutomatedBuild is the top level gate (e.g. if it is false, all the others must be false)
    $isAutomatedBuild = [System.Convert]::ToBoolean($env:CI) `
                        -or [System.Convert]::ToBoolean($env:APPVEYOR) `
                        -or [System.Convert]::ToBoolean($env:GITHUB_ACTIONS)

    if( $isAutomatedBuild )
    {
        # PR and release builds have externally detected indicators that are tested
        # below, so default to a CiBuild (e.g. not a PR, And not a RELEASE)
        $currentBuildKind = [BuildKind]::CiBuild

        # IsPullRequestBuild indicates an automated buddy build and should not be trusted
        $isPullRequestBuild = $env:GITHUB_BASE_REF -or $env:APPVEYOR_PULL_REQUEST_NUMBER

        if($isPullRequestBuild)
        {
            $currentBuildKind = [BuildKind]::PullRequestBuild
        }
        else
        {
            if([System.Convert]::ToBoolean($env:APPVEYOR))
            {
                $isReleaseBuild = $env:APPVEYOR_REPO_TAG
            }
            elseif([System.Convert]::ToBoolean($env:GITHUB_ACTIONS))
            {
                $isReleaseBuild = $env:GITHUB_REF -like 'refs/tags/*'
            }

            if($isReleaseBuild)
            {
                $currentBuildKind = [BuildKind]::ReleaseBuild
            }
        }
    }

    return $currentBuildKind
}
