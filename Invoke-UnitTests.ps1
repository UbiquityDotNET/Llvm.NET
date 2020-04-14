try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment

    $testsFailed = $false

    Write-Information 'Running Source tests as x64'
    $testsFailed = $testsFailed -or (Invoke-DotNetTest $buildInfo 'src\Interop\InteropTests\InteropTests.csproj')

    Write-Information 'Running Core library tests as x64'
    $testsFailed = $testsFailed -or (Invoke-DotNetTest $buildInfo 'src\Ubiquity.NET.Llvm.Tests\Ubiquity.NET.Llvm.Tests.csproj')


    Write-Information 'Running tests for Kaleidoscope Samples as x64'
    $testsFailed = $testsFailed -or (Invoke-DotNetTest $buildInfo 'Samples\Kaleidoscope\Kaleidoscope.Tests\Kaleidoscope.Tests.csproj')

    Write-Information 'Running sample app for .NET Core'
    pushd (Join-path $buildInfo['BuildOutputPath'] bin\CodeGenWithDebugInfo\Release\netcoreapp3.1)
    try
    {
        dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c' $buildInfo['TestResultsPath']
        $testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
    }
    finally
    {
        popd
    }

    if($testsFailed)
    {
        throw "One or more tests failed - Build should fail"
    }
}
catch
{
    Write-Error $_.Exception.Message
}

