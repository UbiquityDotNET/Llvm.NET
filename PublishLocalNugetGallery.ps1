param([string]$BuildOutputRoot)

import-module "Microsoft.TeamFoundation.DistributedTask.Task.Internal"
import-module "Microsoft.TeamFoundation.DistributedTask.Task.Common"

Find-Files -SearchPattern "BuildOutput\Nuget\**\*.nupkg" | Write-Verbose
