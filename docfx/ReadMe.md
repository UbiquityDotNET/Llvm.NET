# Documentation
DocFX is used to generate the documentation for this library. There is confusion on
what the statictoc template means and requires. It is ***LITERALLY*** that the Table
of Contents (TOC) is staticly generated. So that the entire site is servable from a
file path. This ***DOES NOT*** mean that the default+modern template is unusable for
hosted static site scenarios like 'gh-pages' in GitHub. It only means that the TOC
support will ***require*** a hosted site to provide the contents needed by the generated
TOC client side scripting. That's it. Don't fear the built-in templates (Despite the lack 
of decent docs explaining the details [Yeah, this project previously fell into those gaps
and even constructed a custom template to deal with it... Sigh, what a waste of time...
:facepalm: ])

DocFX has obsoleted the `docfxconsole` NuGet pacakge that was used to run docfx for
a project via MSBUILD. Instead it focused on a .NET tool to do it all via the
command line. Ultimately the docfx.json serves as the "project" file for the
different site builds. The PowerShell script `Build-Docs.ps1` was updated to use the
new tool directly. Using that script should have little or no impact on the overall
flow.

There are two "levels" to the docs for this project to allow the old docs to continue
to exist.

1) Index
    1) This is a small thin layer that provides a general overview and links to the
       docs for older versions.
    2) Prior documentation is retained in folders following the `v.<Major>.<minor>.<Patch>`
       patttern.
2) Current documentation
    1) This contains all of the current documentation
