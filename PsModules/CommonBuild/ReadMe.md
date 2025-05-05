# COMMON Build
This contains the common build support for the Ubiquity.NET family of projects. This
PowerShell module is common to ALL projects and repossitories, and therfore ***MUST NOT***
contain any repository specific support.

The general pattern is to use a VERY common script as the top level that will "dot source"
all the real functionality and export any of the public functions in those scripts. The
script source code is organized into three folders:
1) This folder
    - This contains the core PSD1 and PSM1 files that don't need to change as things are
      updated unless adding new exported functions. Then the PSD1 should include those
      functions so that Powershell can load the module when used.
2) Public Folder
    - This folder contains all of the public "exported" functions. Most functions have a
      1:1 relationship with the name of the file, but occasionally multiple related functions
      are grouped into a single source file. To help maintain clarity on that, the scripts
      that contain a single function are named for the function following standard Powershell
      guidelines for function names. Scripts that contain multiple related functions or type
      declarations do not use the standard `<verb>-<noun>` naming pattern (Though the functions
      they contain do)
3) Private folder
    - This folder contains the private functions that are local to this module and not
      exported from it. These are considers internal helpers and implementation details.

## Credits
The layout and design of this module was inspired by an [answer](https://stackoverflow.com/a/44512990)
to a StackOverflow question and a linked
[blog article](https://ramblingcookiemonster.github.io/Building-A-PowerShell-Module/).
The implementation here is a unique expression of a generalized form of the ideas presented
in those sources.
