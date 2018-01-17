# Namespaces
This library is divided into several namespaces based on general use and purpose

#### [Llvm.NET](xref:Llvm.NET)
This is the top level namespace containing the basic classes and types for initializing and
shutting down the underlying LLVM library.

#### [Llvm.NET.DebugInfo](xref:Llvm.NET.DebugInfo)
This namespace contains the Debug metadata support for defining debug source information in generated code.

#### [Llvm.NET.Instructions](xref:Llvm.NET.Instructions)
This namespace contains the instruction classes and instruction builder for the LLVM IR instructions

#### [Llvm.NET.JIT](xref:Llvm.NET.JIT)
This namespace contains the support for the LLVM Just-In-Time compilation engine.

#### [Llvm.NET.Transforms](xref:Llvm.NET.Transforms)
This namespace contains the support for the pass manager and various code transformation passes.

#### [Llvm.NET.Types](xref:Llvm.NET.Types)
This namespace contains the support for defining and querying LLVM types

#### [Llvm.NET.Values](xref:Llvm.NET.Values)
This namespace contains support for manipulating LLVM Values and the hierarchy of value object types
