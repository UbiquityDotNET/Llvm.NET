try
{
    . ./buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment

    if ($env:OUTPUT_LLVM -eq "true") {
        Write-Host '##vso[task.logissue type=warning;]Exiting early after building LLVM'
        return
    }

    Write-Information 'Running Interop tests as x64'
    Invoke-DotNetTest $buildInfo 'src/Interop/InteropTests/InteropTests.csproj' $env:BUILD_CONFIG

    Write-Information 'Running Core library tests as x64'
    Invoke-DotNetTest $buildInfo 'src/Ubiquity.NET.Llvm.Tests/Ubiquity.NET.Llvm.Tests.csproj' $env:BUILD_CONFIG

    Write-Information 'Running tests for Kaleidoscope Samples as x64'
    Invoke-DotNetTest $buildInfo 'Samples/Kaleidoscope/Kaleidoscope.Tests/Kaleidoscope.Tests.csproj' $env:BUILD_CONFIG

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
    $env:LD_DEBUG = $null
    Write-Host "##vso[task.logissue type=error;]Invoke-UnitTests.ps1 failed: $($_.Exception.Message)"
    Write-Error $_.Exception.Message
}

