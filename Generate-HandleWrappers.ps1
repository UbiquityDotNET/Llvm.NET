<#
.SYNOPSIS
    Generates the wrappers for handles from the headers in the Ubiquity.NET.LibLLVM NuGet Package

.DESCRIPTION
    This is used 'off-line' (by a developer) to generate the source for the handle wrappers. It is
    NOT run during an automated build as the generator itself is dependent on the CppSharp library
    that only supports the X64 architecture. Automated builds may (at some point in the future)
    run on any architecture supported by .NET so cannot generate the sources at build time. A developer
    machine generating the wrappers is assumed X64 (Windows, Linux, or Mac)
#>

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    $ErrorActionPreference = 'stop'
    $generatorProj = '.\src\Interop\LlvmBindingsGenerator\LlvmBindingsGenerator.csproj'
    $rspPath = Join-Path $PSScriptRoot 'generator.rsp'

    Write-Information "Generating response file: $rspPath via GenerateResponseFile target in $generatorProj"
    dotnet msbuild -restore -target:GenerateResponseFile -property:HandleGeneratorResponeFilePath=`""$rspPath"`" $generatorProj

    Write-Information 'Generating Handle wrapper source...'
    dotnet run --no-restore --project $generatorProj -- @$rspPath
}
catch
{
    # Everything from the official docs to the various articles in the blog-sphere say this isn't needed
    # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
    # information is retained and the error reported will include the correct source file and line number
    # data for the error. Without this, only the error message is retained and the location information is
    # Line 1, Column 1, of the outer most script file, which is, of course, completely useless.
    throw
}
finally
{
    Pop-Location
    $env:Path = $oldPath
}
