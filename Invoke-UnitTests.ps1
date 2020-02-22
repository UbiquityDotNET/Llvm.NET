Param(
    [Parameter(ParameterSetName='FullBuild')]
    $BuildInfo
)
. .\buildutils.ps1
Initialize-BuildEnvironment

$buildPaths = Get-BuildPaths $PSScriptRoot

if(!$BuildInfo)
{
    $BuildInfo = Get-BuildInformation $buildPaths
}

$testsFailed = $false

Write-Information 'Running tests as x64'
$testProj = Join-Path $buildPaths.SrcRoot 'Llvm.NETTests\Llvm.NET.Tests.csproj'
$runSettings = Join-Path $buildPaths.SrcRoot 'x64.runsettings'
dotnet test $testProj -s $runSettings --no-build --no-restore --logger "trx" -r $buildPaths.TestResultsPath
$testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
if ("${ENV:APPVEYOR_JOB_ID}" -ne "")
{
    $wc = New-Object 'System.Net.WebClient'
    $JobUrl = "https://ci.appveyor.com/api/testresults/mstest/$($env:APPVEYOR_JOB_ID)"
    dir (Join-Path $buildPaths.TestResultsPath '*.trx') | %{ $wc.UploadFile( $JobUrl, $_) }
}

Write-Information 'Running sample app for .NET Core'
if($env:APPVEYOR)
{
    Add-AppVeyorTest -Name 'CodeGenWithDebugInfo (CoreCLR)' -Framework 'CORECLR' -FileName 'CodeGenWithDebugInfo.exe' -Outcome Running
}

pushd (Join-path $buildPaths.BuildOutputPath bin\CodeGenWithDebugInfo\Release\netcoreapp3.1)
try
{
    dotnet CodeGenWithDebugInfo.dll M3 'Support Files\test.c' $buildPaths.TestResultsPath
    $testsFailed = $testsFailed -or ($LASTEXITCODE -ne 0)
    $outcome = @('Failed','Passed')[($LASTEXITCODE -eq 0)]

    Write-Information "CodeGenWithDebugInfo (CoreCLR) - $outcome"
    if($env:APPVEYOR)
    {
        Update-AppVeyorTest -Name 'CodeGenWithDebugInfo (CoreCLR)' -Framework 'CORECLR' -FileName 'CodeGenWithDebugInfo.exe' -Outcome $outcome
    }
}
finally
{
    popd
}

if($testsFailed)
{
    throw "One or more tests failed - Build should fail"
}
