function Find-VSInstance
{
    $setup = Get-Module VSSetup -ListAvailable
    if(!$setup)
    {
        Install-Module VSSetup -Scope CurrentUser -Force | Out-Null
    }
    return Get-VSSetupInstance -All | select -First 1
}

# Main Script entry point -----------
if( $env:APPVEYOR )
{
    $loggerArgs = '/logger:Appveyor'
}

$testsFailed = $false
$TestResultPath = Join-Path (Resolve-Path .\BuildOutput) Test-Results

Write-Information 'Running tests as x64'
dotnet test .\src\Llvm.NETTests\Llvm.NET.Tests.csproj -s (Resolve-Path .\src\x64.runsettings) --no-build --no-restore --logger "trx" -r $TestResultPath
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
if ("${ENV:APPVEYOR_JOB_ID}" -ne "")
{
    $wc = New-Object 'System.Net.WebClient'
    $JobUrl = "https://ci.appveyor.com/api/testresults/mstest/$($env:APPVEYOR_JOB_ID)"
    dir .\BuildOutput\Test-Results\*.trx | %{ $wc.UploadFile( $JobUrl, $_) }
}

Write-Information 'Running sample app for net47'
if($APPVEYOR)
{
    Add-AppVeyorTest -Name 'CodeGenWithDebugInfo (net47)' -Outcome Running
}
pushd '.\BuildOutput\bin\CodeGenWithDebugInfo\Release\net47'
.\CodeGenWithDebugInfo.exe M3 'Support Files\test.c'
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
$outcome = @('Passed','Failed')[($LASTEXITCODE -eq 0)]
if($APPVEYOR)
{
    Update-AppVeyorTest -Name -Name 'CodeGenWithDebugInfo (net47)' -Outcome $outcome
}
popd

Write-Information 'Running sample app for .NET Core'
if($APPVEYOR)
{
    Add-AppVeyorTest -Name 'CodeGenWithDebugInfo (CoreCLR)' -Outcome Running
}
pushd '.\BuildOutput\bin\CodeGenWithDebugInfo\Release\netcoreapp2.1'
dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c'
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
$outcome = @('Passed','Failed')[($LASTEXITCODE -eq 0)]
if($APPVEYOR)
{
    Update-AppVeyorTest -Name -Name 'CodeGenWithDebugInfo (CoreCLR)' -Outcome $outcome
}
popd

if($testsFailed)
{
    throw "One or more tests failed - Build should fail"
}
