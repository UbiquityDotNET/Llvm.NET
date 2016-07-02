## Llvm.NET
![Build status](https://telliam.visualstudio.com/DefaultCollection/_apis/public/build/definitions/fb2ef014-95d6-4df2-a906-2b1187e8f36f/2/badge)

### Welcome to Llvm.NET!
Llvm.NET provides LLVM language and runtime bindings for .NET based applications. Llvm.NET's goal is to provide as robust Class library that
accurately reflects the underlying LLVM C++ model. This is done through an extend LLVM-C API bundled as a native windows DLL (LibLLVM.DLL). Llvm.NET
uses the support of LibLLVM to gain access to the LLVM class library and project it into a .NET managed library that reflects the original class library
design.  
The goal is to match the original class model as closely as possible, while providing an object model to .NET applications that feels familiar and consistent
with common styles and patterns in .NET Framework applications. Thus, while class, method and enumeration names are similar to their counterparts in LLVM, they
are not always identical.

Documentation for the Llvm.NET class library is located at [http://netmf.github.io/Llvm.NET](http://netmf.github.io/Llvm.NET)

### Why Llvm.NET?
Llvm.NET was developed as a means to leverage LLVM as the back-end for an Ahead-Of-Time (AOT) compilation tool for .NET applications targeting micro controllers
(e.g. An AOT compiler for the [.NET Micro Framework](http://www.netmf.com) ). The initial proof of concept built on Llvm.NET was successful in delivering on a
basic application that could implement the micro controller equivalent of the classic "Hello World!" application (e.g. Blinky - an app that blinks an LED) using
LLVM as the back-end code generator. This led to the revival of a former project doing AOT with its own code generator that was tied to the ARMv4 Instruction set.
Thus the [Llilum](https://www.github.com/netmf/Llilum) project was born. Llvm.NET has continued to evolve and improve along with Llilum, though it remains a distinct
project as it has no dependencies on Llilum or any of its components. Llvm.NET is viable for any .NET applications wishing to leverage the functionality of the LLVM
libraries from .NET applications.

### Brief History
Llvm.NET began with LLVM 3.4 as a C++/CLI wrapper which enabled a closer binding to the original C++ object model then the official LLVM-C API supported.
As Llvm.NET progressed so to did LLVM. Eventually the LLVM code base migrated to requiring C++11 support in the language to build. This became an issue for
the C++/CLI wrapper as the Microsoft C++/CLI compiler didn't support the C++11 syntax. Thus a change was made to Llvm.NET to move to an extended C API with
a C# adapter layer to provide the full experience .NET developers expect. While the transition was a tedious one very little application code required changes.

### Platform Support
Currently LLVM.NET supports Win32 and x64 buids targeting the full desktop framework v4.5. To keep life simpler the nuget package contains both native platform
binaries and Llvm.NET itself is built for the "AnyCPU" platform. Llvm.NET contains code to dynamically detect the platform it is running on and load the appropriate
DLL. This allows applications to build for AnyCPU without creating multiple build configurations and release vehicles for applications. However, if your application
has other needs that require a specific platform target, then LlVM.NET can still function.

### Building Llvm.NET
#### Pre-requsites
* Download [LLVM source for 3.8.0 ](http://llvm.org/releases/3.8.0/llvm-3.8.0.src.tar.xz) (At this time LLVM is 3.8.0 )
  * You will need a tool to extract files from that archive format on Windows the recommended tool is [7zip](http://7-zip.org/)
* Build of LLVM libraries  
To Build the LLVM libraries you can use the [BuildLlvmWithVS](https://github.com/NETMF/Llvm.NET/blob/dev/src/LibLLVM/BuildLlvmWithVS.cmd) command script provided.
This script should be placed into the root of your LLVM 3.8 source tree. Run BuildLlvmWithVS from an elevated command prompt to build the required LLVM libraries and
create a registry entry so that the Llvm.NET projects can find the pre-built libraries. (NOTE: On a typical developer machines the full LLVM build takes approximately 6
hours so letting it run overnight or when you are otherwise away from your computer is usually a good idea. Fortunately this only needs to be done once for a given release
of LLVM.)

#### Using Visual Studio
The repository contains a Visual Studio solution file that allows building Llvm.NET and LibLLVM for a single platform configuration, as well as running the available
unit tests. This is the primary mode of working with the Llvm.NET source code duing development. 

#### Replicating the automated build
The Automated build support for Llvm.NET uses BuildAll.slnproj to build all the binaries, sign them [signing not yet supported], and generate a nuget package. To build 
the full package simply run `msbuild BuildAll.slnproj`

#### Sample Application
The [TestDebugInfo](https://github.com/NETMF/Llvm.NET/tree/dev/src/Llvm.NETTests/TestDebugInfo) sample application provides an example of using Llvm.NET to generate
LLVM Bit code equivalent to what the Clang compiler generates for a [simple C language file](https://github.com/NETMF/Llvm.NET/blob/dev/src/Llvm.NETTests/TestDebugInfo/test.c).
TestDebugInfo doesn't actually parse the source, instead it is a manually constructed and documented example of how to use Llvm.NET to accomplish the bit-code generation. 

 

