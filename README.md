## Ubiquity.NET.Llvm

#### Build Status
[![CI-Build](https://github.com/UbiquityDotNET/Llvm.NET/workflows/CI-Build/badge.svg?branch=master&event=push)](https://github.com/UbiquityDotNET/Llvm.NET/actions?query=workflow%3ACI-Build+branch%3Amaster+is%3Ain_progress)
![Release-Build](https://github.com/UbiquityDotNET/Llvm.NET/workflows/Release-Build/badge.svg)
### NuGet
[![NuGet](https://img.shields.io/nuget/dt/Ubiquity.NET.Llvm.svg)](https://www.nuget.org/packages/Ubiquity.NET.Llvm/)  
**NOTE:** until there is an official release of the LLVM10 package with the new name this will indicate the package isn't found, to get the latest official release, use the [master branch](https://github.com/UbiquityDotNET/Llvm.NET/tree/master)

For details of releases, see the [release notes](https://github.com/UbiquityDotNET/Llvm.NET/blob/develop/docfx/ReleaseNotes.md)

### Welcome to Ubiquity.NET.Llvm!
Ubiquity.NET.Llvm provides LLVM language and runtime bindings for .NET based applications. Ubiquity.NET.Llvm's goal is to provide
a robust Class library that accurately reflects the underlying LLVM C++ model. This is done through an extended
LLVM-C API bundled as a native windows DLL (LibLLVM.DLL). Ubiquity.NET.Llvm uses the support of LibLLVM to gain access
to the LLVM class library and project it into a .NET managed library that reflects the original class library
design.  
The goal is to match the original class model as closely as possible, while providing an object model to .NET
applications that feels familiar and consistent with common styles and patterns in .NET Framework applications.
Thus, while class, method and enumeration names are similar to their counterparts in LLVM, they are not always
identical.

>NOTE:
>The name of the library (and Nuget packages changed with V10.0) Instead of Llvm.NET* it is now Ubiquity.NET.Llvm*.
>This helps reflect the owning organization better and also helps in identifying a shift in the licensing (LLVM itself
>changing the license for v10*)

### Brief History
Ubiquity.NET.Llvm was initially developed as a means to leverage LLVM as the back-end for an Ahead-Of-Time (AOT) compilation
tool for .NET applications targeting micro-controllers (e.g. An AOT compiler for the [.NET Micro Framework](http://www.netmf.com) ).
The initial proof of concept built on Ubiquity.NET.Llvm was successful in delivering on a basic application that could
implement the micro controller equivalent of the classic "Hello World!" application (e.g. Blinky - an app that
blinks an LED) using LLVM as the back-end code generator. This led to the revival of a former project doing AOT
with its own code generator that was tied to the ARMv4 Instruction set. ([Llilum](https://www.github.com/netmf/Llilum)).
Ubiquity.NET.Llvm has continued to evolve and improve and remains a distinct project as it has no dependencies on Llilum
or any of its components. Ubiquity.NET.Llvm is viable for any .NET applications wishing to leverage the functionality of
the LLVM libraries from .NET applications.

Ubiquity.NET.Llvm began with LLVM 3.4 as a C++/CLI wrapper which enabled a closer binding to the original C++ object model
then the official LLVM-C API supported. As Ubiquity.NET.Llvm progressed so to did LLVM. Eventually the LLVM code base
migrated to requiring C++/11 support in the language to build. This became an issue for the C++/CLI wrapper as the
Microsoft C++/CLI compiler didn't support the C++11 syntax. Thus a change was made to Ubiquity.NET.Llvm to move to an extended
C API with a C# adapter layer to provide the full experience .NET developers expect. While the transition was a
tedious one very little application code required changes. LLVM and Ubiquity.NET.Llvm have continued to progress and Ubiquity.NET.Llvm
is currently based on LLVM 8.0.

### Platform Support
Currently Ubiquity.NET.Llvm supports x64 builds targeting the full desktop framework v4.7 and .NET standard 2.0. Ideally
other platforms are possible in the future. To keep life simpler the Ubiquity.NET.Llvm NuGet package is built for the "AnyCPU"
platform and references the Ubiquity.NET.Llvm.Interop package to bring in the native binary support. Ubiquity.NET.Llvm.Interop
contains code to dynamically detect the platform it is running on and load the appropriate native DLL. This allows applications
to build for AnyCPU without creating multiple build configurations and release vehicles for applications. (Any new platforms
would need to update the dynamic loading support and include the appropriate P/Invokable binaries - consuming apps would not
need to change except to pick up a new package version.)

### CI Build NuGet Packages
The CI Builds of the NuGet package built from the latest source in the master branch are available as build artifacts from the build. 
Unfortunately with an all GitHub build via GitHub Actions we don't have a good story for accessing the packages from unreleased automated builds. While GitHub does support a package registry (GPR), it really doesn't meet the needs of CI builds. In particular:
* GPR Doesn't support deletion of older CI build packages (Cluttering the feed)
* GPR requires complex login/Tokens just to get packages from the feed, despite being a public repository...
* Tool integration (esp. Visual Studio) is not well supported and difficult to setup.

Given all of the above the CI builds are not published to a feed at this time and GPR isn't used for publishing releases. (Official NuGet will serve that role for releases)
CI build and PR build packages are available as artifacts from the GitHub actions that build them.

### API Documentation
The full API documentation on using Ubiquity.NET.Llvm is available on the [Ubiquity.NET.Llvm documentation site](https://ubiquitydotnet.github.io/Ubiquity.NET.Llvm/).

### Sample Applications
#### Code Generation With Debug Information
The [CodeGenWithDebugInfo](https://github.com/UbiquityDotNET/Llvm.NET/tree/master/Samples/CodeGenWithDebugInfo) sample application provides an example of using Ubiquity.NET.Llvm to generate
LLVM Bit code equivalent to what the Clang compiler generates for a [simple C language file](https://github.com/UbiquityDotNET/Llvm.NET/blob/master/Samples/CodeGenWithDebugInfo/Support%20Files/test.c).
The sample application doesn't actually parse the source, instead it is a manually constructed and documented example of how to use Ubiquity.NET.Llvm to accomplish the bit-code generation. 

#### Kaleidoscope Tutorial
A Ubiquity.NET.Llvm version of the LLVM sample [Kaleidoscope language tutorial](https://ubiquitydotnet.github.io/Llvm.NET/articles/Samples/Kaleidoscope.html) is provided to walk through many aspects of code generation and JIT execution with Llvm.NET. This tutorial implements a complete JIT execution engine for the Kaleidoscope language, along with AOT compilation, optimization and debug symbol generation. This, covers a significant surface area of the Llvm.NET classes and methods to provide a solid grounding on the use of the library.

## Building Ubiquity.NET.Llvm
### Prerequisites
* Visual Studio 2019 (16.4+) [Community Edition OK]
* [7-Zip](https://www.7-zip.org/) [Used to unpack the pre-built LLVM libraries]

#### Using Visual Studio
The repository contains Visual Studio solution files that allow building the components individually for modifying
Ubiquity.NET.Llvm and LibLLVM, as well as running the available unit tests. This is the primary mode of working with the
Ubiquity.NET.Llvm source code during development.

### Replicating the automated build
The Automated build support for Ubiquity.NET.Llvm uses Build-All.ps1 PowerShell script to build all the binaries and generate a
NuGet package. To build the full package simply run `Build-All.ps1 -ForceClean` from a PowerShell command prompt with MSBuild tools
on the system search path.

### Code of Conduct
This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community. For more information, see the
[.NET Foundation Code of Conduct.](http://www.dotnetfoundation.org/code-of-conduct)

 

