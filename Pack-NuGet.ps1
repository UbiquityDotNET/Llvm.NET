. .\buildutils.ps1
$buildInfo = Initialize-BuildEnvironment

Push-Location $buildInfo['NuGetOutputPath']
Compress-Archive -Force -Path *.* -DestinationPath (join-path $buildInfo['BuildOutputPath'] Nuget.Packages.zip)
Pop-Location
