using module "PSModules/CommonBuild/CommonBuild.psd1"
using module "PSModules/RepoBuild/RepoBuild.psd1"

<#
.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.
#>
Param(
    [switch]$FullInit
)

$buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit

Write-Information "Preparing to commit updated docs for gh-pages"

$canCommit = $env:IsAutomatedBuild -and ($env:IsPullRequestBuild -ieq 'false')
if(!$canCommit)
{
    Write-Information "Skipping Docs commit as this is not an official build"
    return;
}

Push-Location .\BuildOutput\docs -ErrorAction Stop
try
{
    Invoke-External git config --local user.email "$env:docspush_email"
    Invoke-External git config --local user.name "$env:docspush_username"

    # This repo uses docfx to generate the site content, not Jekyll the site MUST include
    # a '.nojekyll' file. This is apparently true for EVERY push, but isn't entirely clear
    # as there is almost no documentation of the existence of such a thing, let alone how
    # to use it. This makes it a VERY annoying and poorly documented 'feature'. (Disabling
    # Jekyll should be a repo setting done once for a repo or organization.)
    [DateTime]::UtcNow.ToString('o') | Out-File .nojekyll

    Write-Information 'Adding files to git'
    Invoke-External git add -A
    Invoke-External git ls-files -o --exclude-standard | %{ git add $_}

    $msg = "Automated Build Docs Update $(Get-BuildVersionTag $buildInfo)"
    Write-Information "Committing changes to git [$msg]"
    Invoke-External git commit -m"$msg"
}
catch
{
    # everything from the official docs to the various articles in the blog-sphere says this isn't needed
    # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
    # information is retained and the error reported will include the correct source file and line number
    # data for the error. Without this, only the error message is retained and the location information is
    # Line 1, Column 1, of the outer most script file, which is, of course, completely useless.
    throw
}

finally
{
    Pop-Location
}
