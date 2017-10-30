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
$vsInstance = Find-VSInstance
$vstest = [System.IO.Path]::Combine($vsInstance.InstallationPath, 'Common7','IDE','CommonExtensions','Microsoft','TestWindow','vstest.console.exe')

Write-Information 'Running tests as win32'
& $vstest .\BuildOutput\bin\Llvm.NETTests\Release\net47\Llvm.NETTests.dll /InIsolation /Settings:src\win32.runsettings $loggerArgs

Write-Information 'Running tests as x64'
& $vstest .\BuildOutput\bin\Llvm.NETTests\Release\net47\Llvm.NETTests.dll /InIsolation /Settings:src\x64.runsettings $loggerArgs
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)

Write-Information 'Running sample app for net47'
if($APPVEYOR)
{
    Add-AppVeyorTest -Name 'CodeGenWithDebugInfo (net47)' -Outcome Running
}
pushd '.\BuildOutput\bin\CodeGenWithDebugInfo\Release\net47'
.\CodeGenWithDebugInfo.exe 'Support Files\test.c'
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
pushd '.\BuildOutput\bin\CodeGenWithDebugInfo\Release\netcoreapp2.0'
dotnet CodeGenWithDebugInfo.dll 'Support Files\test.c'
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
