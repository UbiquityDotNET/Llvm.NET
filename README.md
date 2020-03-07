## Llvm.NET

#### Build Status
[![CI-Build](https://github.com/UbiquityDotNET/Llvm.NET/workflows/CI-Build/badge.svg?branch=master&event=push)](https://github.com/UbiquityDotNET/Llvm.NET/actions?query=workflow%3ACI-Build+branch%3Amaster+is%3Ain_progress)
![Release-Build](https://github.com/ubiquitydotnet/llvm.net/workflows/Release-Build/badge.svg)
### Nuget
[![Nuget](https://img.shields.io/nuget/dt/Llvm.NET.svg)](https://www.nuget.org/packages/Llvm.NET/)

For details of releases, see the [release notes](https://github.com/UbiquityDotNET/Llvm.NET/blob/master/docfx/ReleaseNotes.md)

## Welcome to Llvm.NET!
Llvm.NET provides LLVM language and runtime bindings for .NET based applications. Llvm.NET's goal is to provide
a robust Class library that accurately reflects the underlying LLVM C++ model. This is done through an extended
LLVM-C API bundled as a native windows DLL (LibLLVM.DLL). Llvm.NET uses the support of LibLLVM to gain access
to the LLVM class library and project it into a .NET managed library that reflects the original class library
design.  
The goal is to match the original class model as closely as possible, while providing an object model to .NET
applications that feels familiar and consistent with common styles and patterns in .NET Framework applications.
Thus, while class, method and enumeration names are similar to their counterparts in LLVM, they are not always
identical.

### Why Llvm.NET?
Llvm.NET was initially developed as a means to leverage LLVM as the back-end for an Ahead-Of-Time (AOT) compilation
tool for .NET applications targeting micro-controllers (e.g. An AOT compiler for the [.NET Micro Framework](http://www.netmf.com) ).
The initial proof of concept built on Llvm.NET was successful in delivering on a basic application that could
implement the micro controller equivalent of the classic "Hello World!" application (e.g. Blinky - an app that
blinks an LED) using LLVM as the back-end code generator. This led to the revival of a former project doing AOT
with its own code generator that was tied to the ARMv4 Instruction set. ([Llilum](https://www.github.com/netmf/Llilum)).
Llvm.NET has continued to evolve and improve and remains a distinct project as it has no dependencies on Llilum
or any of its components. Llvm.NET is viable for any .NET applications wishing to leverage the functionality of
the LLVM libraries from .NET applications.

### Brief History
Llvm.NET began with LLVM 3.4 as a C++/CLI wrapper which enabled a closer binding to the original C++ object model
then the official LLVM-C API supported. As Llvm.NET progressed so to did LLVM. Eventually the LLVM code base
migrated to requiring C++/11 support in the language to build. This became an issue for the C++/CLI wrapper as the
Microsoft C++/CLI compiler didn't support the C++11 syntax. Thus a change was made to Llvm.NET to move to an extended
C API with a C# adapter layer to provide the full experience .NET developers expect. While the transition was a
tedious one very little application code required changes. LLVM and LLvm.NET have continued to progress and Llvm.NET
is currently based on LLVM 8.0.

### Platform Support
Currently LLVM.NET supports x64 builds targeting the full desktop framework v4.7 and .NET standard 2.0. Ideally
other platforms are possible in the future. To keep life simpler the Llvm.NET NuGet package is built for the "AnyCPU"
platform and references the LibLLVM.NET package to bring in the native binary support. Llvm.NET contains code to dynamically
detect the platform it is running on and load the appropriate DLL. This allows applications to build for AnyCPU without
creating multiple build configurations and release vehicles for applications. (Any new platforms would need to update the
dynamic loading support and include the appropriate P/Invokable binaries)

### CI Build NuGet Packages
The CI Builds of the NuGet package built from the latest source in the master branch are available as build artifacts from the build. 
Unfortunately with an all GitHub build via GitHub Actions we don't have a good story for accessing the packages from unreleased automated builds. While GitHub does support a package registry (GPR), it really doesn't meet the needs of CI builds. In particular:
* GPR Doesn't support deletion of older CI build packages (Cluttering the feed)
* GPR requires complex logon/Tokens just to get packages from the feed, despite being a public repro...
* Tool integration (esp. Visual Studio) is not well supported and difficult to setup.

Given all of the above the CI builds are not published to a feed at this time and GPR isn't used for publishing releases. (Official Nuget will serve that role)

### API Documentation
The full API documentation on using Llvm.NET is available on the [Llvm.NET documentation site](https://ubiquitydotnet.github.io/Llvm.NET/).

### Sample Applications
#### Code Generation With Debug Information
The [CodeGenWithDebugInfo](https://github.com/UbiquityDotNET/Llvm.Net/tree/master/Samples/CodeGenWithDebugInfo) sample application provides an example of using Llvm.NET to generate
LLVM Bit code equivalent to what the Clang compiler generates for a [simple C language file](https://github.com/UbiquityDotNET/Llvm.Net/blob/master/Samples/CodeGenWithDebugInfo/Support%20Files/test.c).
The sample application doesn't actually parse the source, instead it is a manually constructed and documented example of how to use Llvm.NET to accomplish the bit-code generation. 

#### Kaleidoscope Tutorial
An Llvm.NET version of the LLVM sample [Kaleidoscope language tutorial](https://ubiquitydotnet.github.io/Llvm.NET/articles/Samples/Kaleidoscope.html) is provided to walk through many aspects of code generation and JIT execution with Llvm.NET. This tutorial implements a complete JIT execution engine for the Kaleidoscope language, along with AOT compilation, optimization and debug symbol generation. This, covers a significant surface area of the Llvm.NET classes and methods to provide a solid grounding on the use of the library.

## Building Llvm.NET
### Prerequsites
* Visual Studio 2017 (15.4+) [Community Edition OK]
* [7-Zip](https://www.7-zip.org/) [Used to unpack the pre-built LLVM libraries]

#### Using Visual Studio
The repository contains Visual Studio solution files that allow building the components individually for modifying
Llvm.NET and LibLLVM, as well as running the available unit tests. This is the primary mode of working with the
Llvm.NET source code during development.

### Replicating the automated build
The Automated build support for Llvm.NET uses BuildAll.ps1 PowerShell script to build all the binaries and generate a
NuGet package. To build the full package simply run `BuildAll.ps1` from a PowerShell command prompt with MSBuild tools
on the system search path.

### Code of Conduct
This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community. For more information, see the
[.NET Foundation Code of Conduct.](http://www.dotnetfoundation.org/code-of-conduct)

 

