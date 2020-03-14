# Namespaces
This library is divided into several namespaces based on general use and purpose

#### [Ubiquity.NET.Llvm](xref:Ubiquity.NET.Llvm)
This is the top level namespace containing the basic classes and types for initializing and
shutting down the underlying LLVM library.

#### [Ubiquity.NET.Llvm.DebugInfo](xref:Ubiquity.NET.Llvm.DebugInfo)
This namespace contains the Debug metadata support for defining debug source information in generated code.

#### [Ubiquity.NET.Llvm.Instructions](xref:Ubiquity.NET.Llvm.Instructions)
This namespace contains the instruction classes and instruction builder for the LLVM IR instructions

#### [Ubiquity.NET.Llvm.JIT](xref:Ubiquity.NET.Llvm.JIT)
This namespace contains the support for the LLVM Just-In-Time compilation engine.

#### [Ubiquity.NET.Llvm.Transforms](xref:Ubiquity.NET.Llvm.Transforms)
This namespace contains the support for the pass manager and various code transformation passes.

#### [Ubiquity.NET.Llvm.Types](xref:Ubiquity.NET.Llvm.Types)
This namespace contains the support for defining and querying LLVM types

#### [Ubiquity.NET.Llvm.Values](xref:Ubiquity.NET.Llvm.Values)
This namespace contains support for manipulating LLVM Values and the hierarchy of value object types

#### Ubiquity.NET.Llvm.Interop
This namespace contains the low level interop layer for the native LLVM C API surface
