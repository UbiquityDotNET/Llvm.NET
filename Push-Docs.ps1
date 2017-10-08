if($env:APPVEYOR_PULL_REQUEST_NUMBER)
{
    return;
}

if($env:APPVEYOR)
{
    git config --global credential.helper store
    Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:docspush_access_token):x-oauth-basic@github.com`n"
    git config --global user.email "smaillet@users.noreply.github.com"
    git config --global user.name "smaillet"
}

git add docs/**
git commit -m "[skip ci] CI Docs Update"
git push origin HEAD:master
