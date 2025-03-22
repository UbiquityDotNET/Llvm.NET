# Documentation
DocFX is used to generate the documentation for this library. The template used is
a customized form of the 'statictoc' template to support a "dark" theme and a few
other adjustments. Sadly, the `modern` template requires a dynamic back end wich
eliminates mots OSS use (as they generally use `gh-pages` from GitHub - like this
repo does.) There are two different "sites" built.
1) The main landing site (index) for the project
    1) This is an intentionally small site that provides the main landing page and
       links to older docs versions along with the current version
2) The actual documentation for the current version
    1) This is the real "meat" of the docs

DocFX has obsoleted the `docfxconsole` NuGet pacakge that was used to run docfx for
a project via MSBUILD. Instead it focused on a .NET tool to do it all via the
command line. Ultimately the docfx.json serves as the "project" file for the
different site builds. The PowerShell script `Build-Docs.ps1` was updated to use the
new tool directly. Using that script should have little or no impact on the overall
flow.
