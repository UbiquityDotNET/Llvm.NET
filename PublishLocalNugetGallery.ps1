param([string]$BuildOutputRoot)

Find-Files -SearchPattern "BuildOutput\Nuget\**\*.nupkg" | Write-Verbose
