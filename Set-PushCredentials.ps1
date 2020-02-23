. .\buildutils.ps1

Initialize-BuildEnvironment

if( $IsAutomatedBuild -and !$IsPullRequestBuild )
{
    pushd $BuildInfo.BuildOutputDocs
    try
    {
        if(!$env:docspush_access_token)
        {
            $env:docspush_access_token = $env:GITHUB_TOKEN
        }

        if(!$env:docspush_access_token)
        {
            Write-Information "Adding credentials For GIT"
            git config --local credential.helper store --file "$env:USERPROFILE\.git-credentials"
            Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:docspush_access_token):x-oauth-basic@github.com`n"
        }

        if(!$env:docspush_email)
        {
            Write-Information "Adding user.email configuration"
            git config --local user.email "$env:docspush_email"
        }

        if($env:docspush_username)
        {
            Write-Information "Adding user.name configuration"
            git config --local user.name "$env:docspush_username"
        }
    }
    finally
    {
        popd
    }
}
