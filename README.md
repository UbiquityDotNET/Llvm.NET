## Llvm.NET

Status:  

![Build Status](https://img.shields.io/appveyor/ci/UbiquityDotNet/Llvm-NET/master.svg?style=flat-square)
![Test Status](https://img.shields.io/appveyor/tests/UbiquityDotNet/Llvm-NET/master.svg?style=flat-square)

### Welcome to Llvm.NET!
Llvm.NET provides LLVM language and runtime bindings for .NET based applications. Llvm.NET's goal is to provide
as robust Class library thataccurately reflects the underlying LLVM C++ model. This is done through an extend
LLVM-C API bundled as a native windows DLL (LibLLVM.DLL). Llvm.NET uses the support of LibLLVM to gain access
to the LLVM class library and project it into a .NET managed library that reflects the original class library
design.  
The goal is to match the original class model as closely as possible, while providing an object model to .NET
applications that feels familiar and consistent with common styles and patterns in .NET Framework applications.
Thus, while class, method and enumeration names are similar to their counterparts in LLVM, they are not always
identical.

### Why Llvm.NET?
Llvm.NET was initially developed as a means to leverage LLVM as the back-end for an Ahead-Of-Time (AOT) compilation
tool for .NET applications targeting micro controllers (e.g. An AOT compiler for the [.NET Micro Framework](http://www.netmf.com) ).
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
migrated to requiring C++11 support in the language to build. This became an issue for the C++/CLI wrapper as the
Microsoft C++/CLI compiler didn't support the C++11 syntax. Thus a change was made to Llvm.NET to move to an extended
C API with a C# adapter layer to provide the full experience .NET developers expect. While the transition was a
tedious one very little application code required changes.

### Platform Support
Currently LLVM.NET supports Win32 and x64 buids targeting the full desktop framework v4.7, though it is intended
to support .NET Standard 2.0 so that more platforms are possible in the future. To keep life simpler the Llvm.NET
nuget package is built for the "AnyCPU" platform and references the LibLLVM.NET package to bring in the native
binary support. Llvm.NET contains code to dynamically detect the platform it is running on and load the appropriate
DLL. This allows applications to build for AnyCPU without creating multiple build configurations and release vehicles
for applications.

### Building Llvm.NET
#### Pre-requsites
* Visual Studio 2017 (15.3+)
* Llvm.Libs Nuget Package
  - To build the Llvm.Libs nuget package locally you can use the build support from the [Llvm.Libs ](https://github.com/UbiquityDotNET/Llvm.Libs) repository

#### Using Visual Studio
The repository contains a Visual Studio solution files that allow building the components individually for modifying
Llvm.NET and LibLLVM, as well as running the available unit tests. This is the primary mode of working with the
Llvm.NET source code duing development.

#### Replicating the automated build
The Automated build support for Llvm.NET uses BuildAll.ps1 powershell script to build all the binaries, sign them
[SHA256 hash only at present], and generate a nuget package. To build the full package simply run `BuildAll.ps1`
from a powershell command prompt with msbuild tools on the system search path.

#### Sample Application
The [CodeGenWithDebugInfo](https://github.com/UbiquityDotNET/Llvm.Net/tree/master/Samples/CodeGenWithDebugInfo) sample application provides an example of using Llvm.NET to generate
LLVM Bit code equivalent to what the Clang compiler generates for a [simple C language file](https://github.com/UbiquityDotNET/Llvm.Net/blob/master/Samples/CodeGenWithDebugInfo/Support%20Files/test.c).
The sample applictation doesn't actually parse the source, instead it is a manually constructed and documented example of how to use Llvm.NET to accomplish the bit-code generation. 

#### Code of Conduct
This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community. For more information, see the
[.NET Foundation Code of Conduct.](http://www.dotnetfoundation.org/code-of-conduct)

 

