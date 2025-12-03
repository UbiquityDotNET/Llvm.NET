# TextMate support
This folder provides TextMate Grammar support. It is based on the [HighlightJS](../../../../docfx/templates/Ubiquity/public/Kaleidoscope.js)
grammar used in the documentation template for the site docs. This allows highlighting in
an editor.

## VisualStudio
In Visual Studio copies of sub-folders of this folder can be placed into the user profile
folder `.vs/Extensions` to enable syntax highlighting of the Kaleidoscope
language files.

>[!NOTE]
> For Windows the user profile is available in an environment variable. In PowerShell that
> is accessed as `$env:userprofile`. In classic CMD prompts it is `%USERPROFILE%`. Normally
> it is `C:\Users\<UserName>` but can vary depending on how your system is set up.

Example:
```
📁 %USERPROFILE%
└─📁 .vs
   └─📁 Extensions
      └─📁 Kaleidoscope
         └─📁 syntaxes
            └─📄 Kaleidoscope.plist
```
This will enable syntax highlighting for all Kaleidoscope files (.kls) loaded
after this file is present (any editors without highlighting need to be closed
and re-opened as the set of contributing highlighters is determined once at file load.)
