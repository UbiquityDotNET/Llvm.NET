function Get-GitHubReleases($org, $project)
{
<#
.SYNOPSIS
    Gets a collection of the GitHub releases for a project

.DESCRIPTION
    This function retrieves a collection of releases from a given GitHub organization and project.
    The result is a collection of GitHub releases as JSON data.

.PARAMETER org
    GitHub organization name that owns the project

.PARAMETER project
    GitHub project to retrieve releases from
#>
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    $releases = Invoke-RestMethod -Uri "https://api.github.com/repos/$org/$project/releases"
    foreach($r in $releases)
    {
        $r
    }
}

function Get-GitHubTaggedRelease($org, $project, $tag)
{
<#
.SYNOPSIS
    Gets a specific tagged release for a GitHub project

.DESCRIPTION
    This function retrieves a single tagged release from a given GitHub organization and project.
    The result is a GitHub release as JSON data.

.PARAMETER org
    GitHub organization name that owns the project

.PARAMETER project
    GitHub project to retrieve releases from

.PARAMETER tag
    Tag to find the specific release for
#>

    Get-GithubReleases $org $project | ?{$_.tag_name -eq $tag}
}
