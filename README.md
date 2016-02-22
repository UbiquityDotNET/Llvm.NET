## Llvm.NET
![Build status](https://telliam.visualstudio.com/DefaultCollection/_apis/public/build/definitions/fb2ef014-95d6-4df2-a906-2b1187e8f36f/2/badge)

### Welcome to Llvm.NET!
Llvm.NET provides LLVM language and runtime bindings for .NET based applications. Llvm.NET's goal is to provide as robust Class library that
accurately reflects the underlying LLVM C++ model. This is done through an extend LLVM-C API bundled as a native windows DLL (LibLLVM.DLL). Llvm.NET
uses the support of LibLLVM to gain access to the LLVM class library and project it into a .NET managed library that reflects the original class library.
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
 

