. .\buildutils.ps1
$buildInfo = Initialize-BuildEnvironment

# Push-Location $buildInfo['NuGetOutputPath']
# Compress-Archive -Force -Path *.* -DestinationPath (join-path $buildInfo['ArtifactDrops'] Nuget.Packages.zip)
Move-Item -Path $buildInfo['NugetOutputPath'] -Destination $buildInfo['ArtifactDrops']
# Pop-Location
