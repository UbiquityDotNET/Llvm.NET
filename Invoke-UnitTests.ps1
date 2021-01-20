try
{
    . ./buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment

    if ($env:OUTPUT_LLVM -eq "true") {
        Write-Host '##vso[task.logissue type=warning;]Exiting early after building LLVM'
        return
    }

    $additionalArgs = @()
    if ($buildInfo['Platform'] -ne [Platform]::Windows) {
        # Skip the Orc JIT and Obj Files tests on non-Windows since those APIs don't work there.
        # Note that escaping the "!" is needed on *nix systems, but won't work right on Windows.
        $additionalArgs = "--filter", "`"TestCategory\!=OrcJIT&TestCategory\!=ObjFiles`""
    }

    Write-Information 'Running Interop tests as x64'
    Invoke-DotNetTest $buildInfo 'src/Interop/InteropTests/InteropTests.csproj' -additionalArgs $additionalArgs

    Write-Information 'Running Core library tests as x64'
    Invoke-DotNetTest $buildInfo 'src/Ubiquity.NET.Llvm.Tests/Ubiquity.NET.Llvm.Tests.csproj' -additionalArgs $additionalArgs

    Write-Information 'Running tests for Kaleidoscope Samples as x64'
    Invoke-DotNetTest $buildInfo 'Samples/Kaleidoscope/Kaleidoscope.Tests/Kaleidoscope.Tests.csproj'

    Write-Information 'Running sample app for .NET Core'
    pushd (Join-path Samples CodeGenWithDebugInfo)
    try
    {
        dotnet run M3 'Support Files/test.c' $buildInfo['TestResultsPath']
        if ($LASTEXITCODE -ne 0) {
            throw "'dotnet run' of CodeGenWithDebugInfo exited with code: $LASTEXITCODE"
        }
    }
    finally
    {
        popd
    }
}
catch
{
    Write-Host "##vso[task.logissue type=error;]Invoke-UnitTests.ps1 failed: $($_.Exception.Message)"
    Write-Error $_.Exception.Message
}

