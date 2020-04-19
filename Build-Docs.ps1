Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClone
)

pushd $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment

    $msBuildProperties = @{ Configuration = $Configuration
                            LlvmVersion = $buildInfo['LlvmVersion']
                          }

    $docsOutputPath = $buildInfo['DocsOutputPath']
    # clone docs output location so it is available as a destination for the Generated docs content
    # and the versioned docs links can function correctly for locally generated docs
    if(!$NoClone -and !(Test-Path (Join-Path $docsOutputPath '.git') -PathType Container))
    {
        if(Test-Path -PathType Container $docsOutputPath)
        {
            del -Path $docsOutputPath -Recurse -Force
        }

        Write-Information "Cloning Docs repository"
        git clone https://github.com/UbiquityDotNET/Llvm.NET.git -b gh-pages $docsOutputPath -q
        if(!$?)
        {
            throw "Git clone failed"
        }
    }

    # remove all contents from 'current' docs to ensure clean generated docs for this release
    $currentVersionDocsPath = Join-Path $docsOutputPath 'current'
    if(Test-Path -PathType Container $currentVersionDocsPath)
    {
        del -Path $currentVersionDocsPath -Recurse -Force
    }

    $docfxRestoreBinLogPath = Join-Path $buildInfo['BinLogsPath'] Ubiquity.NET.Llvm-docfx-Restore.binlog
    $docfxBuildBinLogPath = Join-Path $buildInfo['BinLogsPath'] Ubiquity.NET.Llvm-docfx-Build.binlog

    Write-Information "Building Docs Solution"
    Invoke-MSBuild -Targets 'Restore' -Project docfx\Ubiquity.NET.Llvm.DocFX.sln -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$docfxRestoreBinLogPath") )
    Invoke-MSBuild -Targets 'Build' -Project docfx\Ubiquity.NET.Llvm.DocFX.sln -Properties $msBuildProperties -LoggerArgs ($buildInfo['MsBuildLoggerArgs'] + @("/bl:$docfxBuildBinLogPath") )
}
catch
{
    Write-Error $_.Exception.Message
}
finally
{
    popd
    $env:Path = $oldPath
}
