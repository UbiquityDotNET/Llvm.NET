# Documentation
>NOTE:
> This file itself does NOT participate in the generated docs output. It documents the input
> to the doc generation and the process for maintainers of this library (Who clearly don't
> have great memories or are otherwise easily confused. :nerd_face:)

DocFX is used to generate the documentation for this library. There is confusion on what the
"statictoc" template means and requires. It is ***LITERALLY*** that, the Table of Contents
(TOC) is statically generated such that the entire site is servable from a file path. This
***DOES NOT*** mean that the default+modern template is unusable for hosted static site
scenarios like 'gh-pages' in GitHub. It only means that the TOC support will
***require*** a hosted site to provide the contents needed by the generated TOC client side
scripting. That's it. Don't fear the built-in templates - despite the lack of decent docs
explaining the details! [Yeah, this project previously fell into those gaps and even
constructed a complete custom template to deal with it... Sigh, what a waste of time...
:facepalm: ]

## Changes Over Time
DocFX has obsoleted the `docfxconsole` NuGet package that was used to run docfx for a
project via MSBUILD. Instead it focused on a .NET tool to do it all via the command line.
Ultimately the docfx.json serves as the corollary to a "project" file for the different site
builds. The PowerShell script `Build-Docs.ps1` was updated to use the new tool directly.
Using that script should have little or no impact on the overall flow. There is a
"no-targets" project in the solution to enable easier access to the input files but does not
itself, generate any docs - it's just a placeholder.

## Files used by the docs generation
There are a lot of files used to generate the docs and the concept of a Table of Contents
(TOC) gets confusing fast when using docfx. So this tries to explain them all.

### .gitignore
This marks the files that are generated as ignored for GIT operations (Don't include
generated sources in the repo - the automated build will generate them). Some specific files
are excluded from this but most of the api-* folders are ignored.

### docfx.json
This file serves as the "project" file for docs generation. Sadly, docfx has deprecated old
`docfxconsole` that supported creation of a csproj project file to generate the docs from.
So this file serves as the only equivalent of a project file. Unfortunately, it is in JSON
format and unlike any other project is unusable directly in an IDE as they don't understand
the format of such a thing.

### favicon.ico
This provides the standard web browser icon for the site as a whole.

### index.md
Markdown for the index (Home) of this site.

### toc.yml
YAML file containing the links for the Table of contents for the SITE as a whole (across all
child pages/folders). This is for the TOP row navigation on the site. (It has NOTHING to do
with the left navigation for any given sub folder, other than having the same name,
confusingly.)

>NOTE
> The TOC.YML file format used in these topics is DIFFERENT from what is auto generated.

### Folders
There are a few folders containing input for the site generation.

#### api-*
These folders contain the Generated contents for each project as (YAML) metadata files
parsed and generated from the source.

##### api-*/Index.md
This contains the main landing page for a given library it has the top bar navigation from
[toc.yml](#tocyml) AND the left navigation from the generated
[toc.yml](#generated-left-nav-tocyml) for this API library. (Confusingly, a file with the
same name, but a completely different purpose!)

#### Generated left nav TOC.YML
This version of the overloaded toc.yml file name is used to produce the left navigation for
an API surface. This is generated from the source files and normally contains the set of
namespaces, types, enums etc... Each library project generates it's own version of this file.
Since this is generated it is listed in the [.gitignore](#gitignore) file.

#### Library Content
These folders (named after the `*` portion of the [api-*](#api-*) folder names contains
manually written additional files, articles, samples etc... related to a given library.

## Guide to writing XML DOC comments
When dealing with doc comments the XML can sometimes get in the way of general readability
of the source code. There is an inherent tension between how a particular editor renders the
docs for a symbol/method (VS calls this "Quick Info") and how it is rendered in the final
documentation by a tool like docfx. This guides general use to simplify things as much as
possible.

### Lists
The largest intrusion of the XML into the source is that of lists. The XML doc comments
official support is to use the `<list>` tag. However, that is VERY intrusive and doesn't
easily support sub-lists. Consider:

``` C#
/// Additional steps might include:
/// <para>
/// <list type="number">
///     <item>App specific Validation/semantic analysis</item>
///     <item>Binding of results to an app specific type</item>
///     <item>
///     Act on the results as proper for the application
///     <list type="number">
///         <item>This might include actions parsed but generally isolating the various stages is an easier to understand/maintain model</item>
///         <item>Usually this is just app specific code that uses the bound results to adapt behavior</item>
///     </list>
///     </item>
/// </list>
/// </para>
```

versus:
``` C#
/// Additional steps might include:<br/>
///
/// 1) App specific Validation/semantic analysis<br/>
/// 2) Binding of results to an app specific type<br/>
/// 3) Act on the results as proper for the application<br/>
///     a. This might include actions parsed but generally isolating the various stages is an easier to understand/maintain model<br/>
///     b. Usually this is just app specific code that uses the bound results to adapt behavior<br/>
```

Which one would ***YOU*** rather encounter in code? Which one is easier to understand when
reading the source? This repo chooses the latter. (If you favor the former, perhaps you
should reconsider... :grinning:)

#### How to handle lists
There is little that can be done to alter the rendering of any editor support, at most an
editor might allow specification of a CSS file, but that is the lowest priority of doc
comments. Readability by maintainers of the docs AND the rendering for final docs used by
consumers is of VASTLY higher importance. Still, the editor rendering ***is*** of value to
maintainers so should not be forgotten as it can make a "right mess of things" even if they
render properly in final docs.

##### Guidance
1) ***DO NOT*** use `<para>` tags to include any lists
    a) Doing so will break the docfx rendering that allows for markdown lists
2) Use `</br>' tags to indicate a line break. This is used by the editor rendering to mark
   the end of a line and start a new one. (Stops auto reflow)
3) Accept that the in editor rendering might "trim" the lines it shows, eliminating any
   indentation. [Grrr... Looking at you VS!]
    a) Sadly, there is no avoiding this. Addition of any sort of "markup" to control that
       will interfere with the readability AND the final docs rendering.
4) Always use a different numbering style for sub lists/items
    b) This will at least show in the in-editor rendering as distinct sub items so if
       everything is trimmed it is at least a distinct pattern that is readable.
5) ***DO NOT*** put lists in any place other than inside a `remarks` region
    a) Usually, the remarks comments are not even rendered as the most useful part is the
        API signature and parameter info. Different editors may allow control of that.
        i) In VS [2019|2022] for C# it is controlled by
            `Text Editor > C# > Advanced > Editor Help: "Show remarks in Quick Info."`
            1) Turning this off can greatly reduce the noise AND reduce the problems of
               different rendering as lists are generally not used in the other elements.

