<#
There are are just too many issues and edges that don't work well or at all with
PS classes and modules. So this fakes a class like we used to do in the "Good ol' days"
with "C" before any of the modern OO languages were created.

A CMakeConfig is a hash table with the following properties
| Name              | Type      | Description
|-------------------|-----------|------------|
| Name              | string    | Name of the configuration |
| ConfigurationType | string    | Build configuration of the CMAKE build |
| BuildRoot         | string    | Root directory of the Build (Exctracted from a standard BuildInfo hash table) |
| SrcRoot           | string    | Root directory of the CMAKE specific build |
| Generator         | string    | Generator to use for this CMAKE build |
| CMakeCommandArgs  | ArrayList | Custom args for the CMAKE command |
| CMakeCommandArgs  | ArrayList | Custom args for the CMAKE command |
| BuildCommandArgs  | ArrayList | Additional build specific args to pass to the underlying build-engine |
| InheritEnvironments | ArrayList | Inherited environments for VS CMAKE settings (Windows ONLY)|
| CMakeBuildVariables | hashtable | Table of CMAKE build variables to set |
#>
function Assert-IsCMakeConfig([hashtable]$self)
{
    if(!$self.ContainsKey('Name') -or
       !$self.ContainsKey('ConfigurationType') -or
       !$self.ContainsKey('BuildRoot') -or
       !$self.ContainsKey('SrcRoot') -or
       !$self.ContainsKey('Generator') -or
       !$self.ContainsKey('CMakeCommandArgs') -or
       !$self.ContainsKey('CMakeCommandArgs') -or
       !$self.ContainsKey('BuildCommandArgs') -or
       !$self.ContainsKey('InheritEnvironments') -or
       !$self.ContainsKey('CMakeBuildVariables')
      )
    {
        throw 'Not all properties present in hashtable'
    }

    if ($self['Name'] -isnot [string] -or
        $self['ConfigurationType'] -isnot [string] -or
        $self['BuildRoot'] -isnot [string] -or
        $self['SrcRoot'] -isnot [string] -or
        $self['Generator'] -isnot [string] -or
        $self['CMakeCommandArgs'] -isnot [System.Collections.ArrayList] -or
        $self['CMakeCommandArgs'] -isnot [System.Collections.ArrayList] -or
        $self['BuildCommandArgs'] -isnot [System.Collections.ArrayList] -or
        $self['InheritEnvironments'] -isnot [System.Collections.ArrayList] -or
        $self['CMakeBuildVariables'] -isnot [hashtable]
       )
    {
        throw 'Incorrect property type'
    }
}

function New-CMakeConfig($name, [string]$buildConfiguration, [hashtable]$buildInfo, $cmakeSrcRoot)
{
<#
.SYNOPSIS
    Initializes a new instance of the CMakeConfig data structure

.PARAMETER name
    The name of this configuration. Typically this is of the form `<runtime>-<config>`
    (ex: `x64-Release`). This is used to identify this configuration AND to form
    the output path for it.

.PARAMETER buildInfo
    Hashtable containing the standard build information.

.PARAMETER cmakeSrcRoot
    Root of the CMAKE source files to build
#>
    # start with an empty hash table and build up from there...
    $self = @{}
    if($IsWindows)
    {
        #$self['Generator'] = 'Visual Studio 17 2022'
        $self['Generator'] = 'Ninja'
    }
    else
    {
        # TODO: Select generator for other runtimes...
        throw "Unsupported runtime platform '$([System.Environment]::OSVersion.Platform)'"
    }

    $self['Name']=$name
    $self['ConfigurationType'] = $buildConfiguration
    $self['BuildRoot'] = Join-Path $buildInfo['BuildOutputPath'] $self['Name']
    $self['SrcRoot'] = $cmakeSrcRoot
    $self['CMakeCommandArgs'] = [System.Collections.ArrayList]@()
    $self['BuildCommandArgs'] = [System.Collections.ArrayList]@()
    $self['InheritEnvironments'] = [System.Collections.ArrayList]@()
    $self['CMakeBuildVariables'] = @{}

    # single config generator; Requires setting the configuration type
    # as a build var during generation (Otherwise, debug is assumed...)
    if($self['Generator'] -ieq 'Ninja')
    {
        $self['CMakeBuildVariables']['CMAKE_BUILD_TYPE']=$self['ConfigurationType']
    }

    # Not needed with Ninja builds
    #if($IsWindows)
    #{
    #    # running on ARM64 is not tested or supported
    #    # This might not be needed now that the build is auto configuring the "VCVARS"
    #    # Ninja build might also remove the need for this...
    #    if ([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq "X64" )
    #    {
    #        $self['CMakeCommandArgs'].Add('-Thost=x64') | Out-Null
    #        $self['InheritEnvironments'].Add('msvc_x64_x64') | Out-Null
    #    }
    #    else
    #    {
    #        $self['InheritEnvironments'].Add('msvc_x64') | Out-Null
    #    }
    #
    #    # pass the /m switch to MSBUILD to enable parallel builds on all available CPU cores
    #    $self['BuildCommandArgs'].Add('/m') | Out-Null
    #}

    Assert-IsCMakeConfig $self
    return $self
}

function Invoke-GenerateCMakeConfig([hashtable]$self, [hashtable] $additionalBuildVars = $null)
{
    Assert-IsCMakeConfig $self

    Write-Information "Generating CMake build support for $($self['Name'])"
    # if the output path doesn't exist, create it now
    if(!(Test-Path -PathType Container $self['BuildRoot'] ))
    {
        New-Item -ItemType Container $self['BuildRoot'] | Out-Null
    }

    # Construct full set of CMAKE args from fixed options and configuration variables
    $cmakeArgs = New-Object System.Collections.ArrayList
    $cmakeArgs.AddRange(@('-G', "$($self['Generator'])") ) | Out-Null
    $cmakeArgs.AddRange(@('-B', "$($self['BuildRoot'])") ) | Out-Null
    foreach( $param in $self['CMakeCommandArgs'] )
    {
        $cmakeArgs.Add( $param ) | Out-Null
    }

    foreach( $var in $self['CMakeBuildVariables'].GetEnumerator() )
    {
        Write-Verbose "CMAKE-VAR: $($var.Key)=$($var.Value)"
        $cmakeArgs.Add( "-D$($var.Key)=$($var.Value)" ) | Out-Null
    }

    # add any build instance specific vars provided
    if($additionalBuildVars)
    {
        foreach( $var in $additionalBuildVars.GetEnumerator())
        {
            if ($var.Key -eq $null -or $var.Value -eq $null)
            {
                throw 'Invalid build var provided'
            }

            Write-Verbose "CMAKE-VAR: $($var.Key)=$($var.Value)"
            $cmakeArgs.Add( "-D$($var.Key)=$($var.Value)" ) | Out-Null
        }
    }

    $cmakeArgs.Add( $self['SrcRoot'] ) | Out-Null
    Invoke-TimedBlock "CMAKE Generate" {
        Write-Information  "cmake $($cmakeArgs -join ' ')"

        # Splat the array of args as distinct elements for external invoke
        Invoke-External cmake @cmakeArgs
    }
}

# workaround stupid warning about exported function verbs without an "approved" verb
# in the name. Use a sensible but unapproved alias name instead. [Sigh, what a mess...]
# see: https://github.com/PowerShell/PowerShell/issues/13637
New-Alias -Name Generate-CMakeConfig -Value Invoke-GenerateCMakeConfig

function Build-CmakeConfig([hashtable]$self, $targets)
{
    Assert-IsCMakeConfig $self
    $cmakeArgs = [System.Collections.ArrayList]@('--build', "$($self['BuildRoot'])", '--config', "$($self['ConfigurationType'])")
    if($targets)
    {
        $cmakeArgs.Add('-t') | Out-Null
        $cmakeArgs.AddRange($targets)
    }

    if($self['BuildCommandArgs'].Length -gt 0)
    {
        $cmakeArgs.AddRange( @('--', "$($self['BuildCommandArgs'])") )
    }

    Invoke-TimedBlock "CMake Building $($self['Name'])" {
        Write-Information "cmake $($cmakeArgs -join ' ')"

        # Splat the array of args as distinct items for external invoke
        Invoke-External cmake @cmakeArgs
    }
}
