function Find-OnPath
{
    [CmdletBinding()]
    Param( [Parameter(Mandatory=$True,Position=0)][string]$exeName)
    $path = where.exe $exeName 2>$null
    if(!$path)
    {
        Write-Verbose "'$exeName' not found on PATH"
    }
    else
    {
        Write-Verbose "Found: '$path'"
    }
    return $path
}

# invokes NuGet.exe, handles downloading it to the script root if it isn't already on the path
function Invoke-NuGet
{
    #update system search path to include the directory of this script for NuGet.exe
    $oldPath = $env:Path
    $env:Path = "$PSScriptRoot;$env:Path"
    try
    {
        $NuGetPaths = Find-OnPath NuGet.exe -ErrorAction Continue
        if( !$NuGetPaths )
        {
            # Download it from official NuGet release location
            Invoke-WebRequest -UseBasicParsing -Uri https://dist.NuGet.org/win-x86-commandline/latest/NuGet.exe -OutFile "$PSScriptRoot\NuGet.exe"
        }
        Write-Information "NuGet $args"
        NuGet $args
        $err = $LASTEXITCODE
        if($err -ne 0)
        {
            throw "Error running NuGet: $err"
        }
    }
    finally
    {
        $env:Path = $oldPath
    }
}

function Find-VSInstance
{
    Install-Module VSSetup -Scope CurrentUser | Out-Null
    return Get-VSSetupInstance -All | select -First 1
}

function Find-MSBuild
{
    $foundOnPath = $true
    $msBuildPath = Find-OnPath msbuild.exe -ErrorAction Continue
    if( !$msBuildPath )
    {
        Write-Verbose "MSBuild not found attempting to locate VS installation"
        $vsInstall = Find-VSInstance
        if( !$vsInstall )
        {
            throw "MSBuild not found on PATH and No instances of VS found to use"
        }

        Write-Verbose "VS installation found: $vsInstall"
        $msBuildPath = [System.IO.Path]::Combine( $vsInstall.InstallationPath, 'MSBuild', '15.0', 'bin', 'MSBuild.exe')
        $foundOnPath = $false
    }

    if( !(Test-Path -PathType Leaf $msBuildPath ) )
    {
        Write-Verbose 'MSBuild not found'
        return $null
    }

    Write-Verbose "MSBuild Found at: $msBuildPath"
    return @{ FullPath=$msBuildPath
              BinPath=[System.IO.Path]::GetDirectoryName( $msBuildPath )
              FoundOnPath=$foundOnPath
            }
}

function Invoke-msbuild([string]$project, [hashtable]$properties, [string[]]$targets, [string[]]$loggerArgs=@(), [string[]]$additionalArgs=@())
{ 
    $oldPath = $env:Path
    try
    {
        $msbuildArgs = @($project, "/t:$($targets -join ';')") + $loggerArgs + $additionalArgs
        if( $properties )
        {
            $msbuildArgs += @( "/p:$(ConvertTo-PropertyList $properties)" ) 
        }

        Write-Information "msbuild $($msbuildArgs -join ' ')"
        msbuild $msbuildArgs
        if($LASTEXITCODE -ne 0)
        {
            throw "Error running msbuild: $LASTEXITCODE"
        }
    }
    finally
    {
        $env:Path = $oldPath
    }
}

function Normalize-Path([string]$path)
{
    $path = [System.IO.Path]::GetFullPath($path)
    if( !$path.EndsWith([System.IO.Path]::DirectorySeparatorChar) )
    {
        $path += [System.IO.Path]::DirectorySeparatorChar
    }
    return $path
}

function Get-BuildPaths([string]$repoRoot)
{
    $buildPaths =  @{}
    $buildPaths.RepoRoot = $repoRoot
    $buildPaths.BuildOutputPath = Normalize-Path (Join-Path $repoRoot 'BuildOutput')
    $buildPaths.NuGetRepositoryPath = Normalize-Path (Join-Path $buildPaths.BuildOutputPath 'packages')
    $buildPaths.NuGetOutputPath = Normalize-Path (Join-Path $buildPaths.BuildOutputPath 'NuGet')
    $buildPaths.SrcRoot = Normalize-Path (Join-Path $repoRoot 'src')
    $buildPaths.LibLLVMSrcRoot = Normalize-Path (Join-Path $buildPaths.SrcRoot 'LibLLVM')
    $buildPaths.BuildExtensionsRoot = ([IO.Path]::Combine( $repoRoot, 'BuildExtensions') )
    $buildPaths.GenerateVersionProj = ([IO.Path]::Combine( $buildPaths.BuildExtensionsRoot, 'CommonVersion.csproj') )
    return $buildPaths
}

function Get-BuildInformation($buildPaths)
{
    Write-Information "Restoring NuGet for $buildPaths.GenerateVersionProj"
    invoke-msbuild -Targets Restore -Project $buildPaths.GenerateVersionProj -LoggerArgs $msbuildLoggerArgs

    Write-Information "Computing Build information"
    Invoke-MSBuild -Targets GenerateVersionJson -Project $buildPaths.GenerateVersionProj -LoggerArgs $msbuildLoggerArgs
    
    $semVer = get-content (Join-Path $buildPaths.BuildOutputPath GeneratedVersion.json) | ConvertFrom-Json

    return @{ FullBuildNumber = $semVer.FullBuildNumber
              PackageVersion = $semVer.FullBuildNumber
              FileVersionMajor = $semVer.FileVersionMajor
              FileVersionMinor = $semVer.FileVersionMinor
              FileVersionBuild = $semVer.FileVersionBuild
              FileVersionRevision = $semver.FileVersionRevision
              FileVersion= "$($semVer.FileVersionMajor).$($semVer.FileVersionMinor).$($semVer.FileVersionBuild).$($semVer.FileVersionRevision)"
              LlvmVersion = "5.0.0"
            }
}

function ConvertTo-PropertyList([hashtable]$table)
{
    (($table.GetEnumerator() | %{ "$($_.Key)=$($_.Value)" }) -join ';')
}

# Main Script entry point -----------

pushd $PSScriptRoot
$oldPath = $env:Path
$ErrorActionPreference = "Continue"
$InformationPreference = "Continue"
try
{
    $msbuild = Find-MSBuild
    if( !$msbuild )
    {
        throw "MSBuild not found"
    }

    if( !$msbuild.FoundOnPath )
    {
        $env:Path = "$env:Path;$($msbuild.BinPath)"
    }

    # setup standard MSBuild logging for this build
    $msbuildLoggerArgs = @('/clp:Verbosity=Minimal')

    if (Test-Path "C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll")
    {
        $msbuildLoggerArgs = $msbuildLoggerArgs + @("/logger:`"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll`"")
    }

    $buildPaths = Get-BuildPaths $PSScriptRoot

    Write-Information "Build Paths:"
    Write-Information ($buildPaths | Format-Table | Out-String)

    if( Test-Path -PathType Container $buildPaths.BuildOutputPath )
    { 
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $buildPaths.BuildOutputPath    
    }

    md BuildOutput\NuGet\ | Out-Null

    $BuildInfo = Get-BuildInformation $buildPaths
    if($env:APPVEYOR)
    {
        Update-AppVeyorBuild -Version $BuildInfo.FullBuildNumber
    }
                                
    $packProperties = @{ version=$($BuildInfo.PackageVersion)
                         llvmversion=$($BuildInfo.LlvmVersion)
                         buildbinoutput=(normalize-path (Join-path $($buildPaths.BuildOutputPath) 'bin'))
                         configuration='Release'
                       }

    $msBuildProperties = @{ Configuration = 'Release'
                            FullBuildNumber = $BuildInfo.FullBuildNumber
                            PackageVersion = $BuildInfo.PackageVersion
                            FileVersionMajor = $BuildInfo.FileVersionMajor
                            FileVersionMinor = $BuildInfo.FileVersionMinor
                            FileVersionBuild = $BuildInfo.FileVersionBuild
                            FileVersionRevision = $BuildInfo.FileVersionRevision
                            FileVersion = $BuildInfo.FileVersion
                            LlvmVersion = $BuildInfo.LlvmVersion
                          }

    Write-Information "Build Parameters:"
    Write-Information ($BuildInfo | Format-Table | Out-String)

    # Need to invoke NuGet directly for restore of vcxproj as /t:Restore target doesn't support packages.config
    # and PackageReference isn't supported for native projects
    Write-Information "Restoring NuGet Packages for LibLLVM.vcxproj"
    Invoke-NuGet restore src\LibLLVM\LibLLVM.vcxproj -PackagesDirectory $buildPaths.NuGetRepositoryPath -Verbosity quiet

    Write-Information "Building LibLLVM"
    Invoke-MSBuild -Targets Build -Project src\LibLLVM\MultiPlatformBuild.vcxproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs

    Write-Information "Building LibLLVM"
    Invoke-MSBuild -Targets Pack -Project src\LibLLVM\LibLLVM.vcxproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs

    Write-Information "Restoring NuGet Packages for Llvm.NET"
    Invoke-MSBuild -Targets Restore -Project src\Llvm.NET\Llvm.NET.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs

    Write-Information "Building Llvm.NET"
    Invoke-MSBuild -Targets Build -Project src\Llvm.NET\Llvm.NET.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs

    Write-Information "Running NuGet Restore for Llvm.NET Tests"
    Invoke-MSBuild -Targets Restore -Project src\Llvm.NETTests\LLVM.NETTests.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs

    Write-Information "Building Llvm.NET Tests"
    Invoke-MSBuild -Targets Build -Project src\Llvm.NETTests\LLVM.NETTests.csproj -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs

    Write-Information "Restoring NuGet Packages for Samples"
    Invoke-MSBuild -Targets Restore -Project Samples\Samples.sln -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs

    Write-Information "Building Samples"
    Invoke-MSBuild -Targets Build -Project Samples\Samples.sln -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs -AdditionalArgs @("/m")
}
catch
{
    $Error
}
finally
{
    popd
    $env:Path = $oldPath
}
