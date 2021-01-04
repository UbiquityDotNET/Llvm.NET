try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment

    if ($env:OUTPUT_LLVM -eq "true") {
        Write-Host '##vso[task.logissue type=warning;]Exiting early after building LLVM'
        return
    }

    Write-Information 'Running Interop tests as x64'
    Invoke-DotNetTest $buildInfo 'src\Interop\InteropTests\InteropTests.csproj'

    Write-Information 'Running Core library tests as x64'
    Invoke-DotNetTest $buildInfo 'src\Ubiquity.NET.Llvm.Tests\Ubiquity.NET.Llvm.Tests.csproj'

    Write-Information 'Running tests for Kaleidoscope Samples as x64'
    Invoke-DotNetTest $buildInfo 'Samples\Kaleidoscope\Kaleidoscope.Tests\Kaleidoscope.Tests.csproj'

    Write-Information 'Running sample app for .NET Core'
    pushd (Join-path $buildInfo['BuildOutputPath'] bin\CodeGenWithDebugInfo\Release\netcoreapp3.1)
    try
    {
        dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c' $buildInfo['TestResultsPath']
        if ($LASTEXITCODE -ne 0) {
            throw "'dotnet CodeGenWithDebugInfo.dll' exited with code: $LASTEXITCODE"
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

