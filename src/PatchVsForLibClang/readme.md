# PatchVsForLibClang
The VS headers contain a bug, tracked by [microsoft/STL bug 1300](https://github.com/microsoft/STL/issues/1300),
that causes failures to parse correctly with LibClang, which is used by the LlvmBindingsGenerator.

Unfortunately the fix is not made as a hotpatch as of this time, and is only included in
VS 2019 16.9 Preview 2. This makes it unavailable to automated builds where the hosting platform
doesn't make environments available with preview releases of VS. Fortunately the fix is small,
and the STL team even published the GIT patch file for intrin0.h (the core file that triggers
the issues).

This project contains a program to apply a patch to the existing installation of VS (making a
 backup of the original, of course :grin:)

Usage:  
`PatchVsForLibClang <VsInstallPath>`

Where:  
VsInstallPath is the installation path of a VS instance. Normally this is retrieved in a PowerShell
script from a VS Setup Instance (See the [vssetup.powershell](https://github.com/Microsoft/vssetup.powershell)
repo for more information)

Example:  
`PatchVsForLibClang "D:\Program Files (x86)\Microsoft Visual Studio\2019\Community"`

This will patch the file @"D:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Tools\MSVC\14.28.29333\include\intrin0.h"
creating a backup file (intrin0.h.bak) in the same directory