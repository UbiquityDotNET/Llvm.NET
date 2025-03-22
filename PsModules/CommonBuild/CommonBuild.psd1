@{
# Script module or binary module file associated with this manifest.
RootModule = 'CommonBuild.psm1'

# Version number of this module.
ModuleVersion = '1.1.0'

# ID used to uniquely identify this module
GUID = '6da1bba8-442f-4bc3-9991-9c8783df06a4'

# Author of this module
Author = 'Ubiquity.NET Contributors'

# Company or vendor of this module
CompanyName = 'Ubquity.NET'

# Copyright statement for this module
Copyright = '(c) 2020-2025 Ubiquity.NET Contributors. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Common build support functions for Ubiquity.NET projects'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '7.0'

# PowerShell editions this module is compatible with
CompatiblePSEditions = @('Core')

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module
# DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module
# CLRVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
# RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
# FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module
FunctionsToExport = @(
    'ConvertTo-NormalizedPath',
    'ConvertTo-PropertyList',
    'Expand-7zArchive',
    'Expand-ArchiveStream',
    'Expand-StreamFromUri',
    'Find-7Zip',
    'Find-MSBuild',
    'Find-OnPath',
    'Find-VSInstance',
    'Get-CurrentBuildKind'
    'Get-GitHubReleases',
    'Get-GitHubTaggedRelease',
    'Invoke-TimedBlock',
    'Invoke-MSBuild',
    'Invoke-NuGet',
    'Update-Submodules',
    'Invoke-DotNetTest',
    'Initialize-CommonBuildEnvironment',
    'Show-FullBuildInfo',
    'Invoke-Git',
    'Get-BuildVersionTag',
    'Invoke-DotNet'
)

# Cmdlets to export from this module
CmdletsToExport = '*'

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module
AliasesToExport = '*'

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess
# PrivateData = ''

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''
}

