. .\buildutils.ps1
Initialize-BuildEnvironment

$testsFailed = $false

Write-Information 'Running tests as x64'
$testProj = Join-Path $BuildPaths.SrcRoot 'Ubiquity.NET.Llvm.Tests\Ubiquity.NET.Llvm.Tests.csproj'
$runSettings = Join-Path $BuildPaths.SrcRoot 'x64.runsettings'
dotnet test $testProj -s $runSettings --no-build --no-restore --logger "trx" -r $BuildPaths.TestResultsPath
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)

Write-Information 'Running sample app for .NET Core'
pushd (Join-path $BuildPaths.BuildOutputPath bin\CodeGenWithDebugInfo\Release\netcoreapp3.1)
try
{
    dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c' $BuildPaths.TestResultsPath
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
