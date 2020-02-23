. .\buildutils.ps1
Initialize-BuildEnvironment

$testsFailed = $false

Write-Information 'Running tests as x64'
$testProj = Join-Path $BuildPaths.SrcRoot 'Llvm.NETTests\Llvm.NET.Tests.csproj'
$runSettings = Join-Path $BuildPaths.SrcRoot 'x64.runsettings'
dotnet test $testProj -s $runSettings --no-build --no-restore --logger "trx" -r $BuildPaths.TestResultsPath
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
if ("${ENV:APPVEYOR_JOB_ID}" -ne "")
{
    $wc = New-Object 'System.Net.WebClient'
    $JobUrl = "https://ci.appveyor.com/api/testresults/mstest/$($env:APPVEYOR_JOB_ID)"
    dir (Join-Path $BuildPaths.TestResultsPath '*.trx') | %{ $wc.UploadFile( $JobUrl, $_) }
}

Write-Information 'Running sample app for .NET Core'
if($env:APPVEYOR)
{
    Add-AppVeyorTest -Name 'CodeGenWithDebugInfo (CoreCLR)' -Framework 'CORECLR' -FileName 'CodeGenWithDebugInfo.exe' -Outcome Running
}
pushd (Join-path $BuildPaths.BuildOutputPath bin\CodeGenWithDebugInfo\Release\netcoreapp3.1)
dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c' $BuildPaths.TestResultsPath
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
$outcome = @('Failed','Passed')[($LASTEXITCODE -eq 0)]
if($env:APPVEYOR)
{
    Update-AppVeyorTest -Name 'CodeGenWithDebugInfo (CoreCLR)' -Framework 'CORECLR' -FileName 'CodeGenWithDebugInfo.exe' -Outcome $outcome
}
popd

if($testsFailed)
{
    throw "One or more tests failed - Build should fail"
}
