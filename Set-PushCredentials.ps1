. .\buildutils.ps1

Initialize-BuildEnvironment

if( $IsAutomatedBuild -and !$IsPullRequestBuild )
{
    if(!$env:docspush_access_token)
    {
        Write-Error "Missing docspush_access_token"
    }

    if(!$env:docspush_email)
    {
        Write-Error "Missing docspush_email"
    }

    if(!$env:docspush_username)
    {
        Write-Error "Missing docspush_username"
    }

    # Best effort, on git commands as they can return non-zero even if nothing is wrong.
    $ErrorActionPreference = 'Continue'

    Write-Information "Setting credential.helper"
    git config --global credential.helper store

    Write-Information "Adding credentials to .git-credentials"
    Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:docspush_access_token):x-oauth-basic@github.com`n"

    Write-Information "Adding user.email configuration"
    git config --global user.email "$env:docspush_email"

    Write-Information "Adding user.name configuration"
    git config --global user.name "$env:docspush_username"
}
