---
uid: Ubiquity.NET.Llvm.Types
remarks: *content
---
This namespace contains the support for native types in this library. Of particular interest
is the use of an interface [ITypeRef](xref:Ubiquity.NET.Llvm.Types.ITypeRef) for consumption
of types. This is done to allow consumption of a type with debugging information attached or
just a native LLVM type in the same manner. This is a very useful convenience for code
generation that also helps to solve the "type tracking of pointers" to support the new LLVM
opaque pointer model.
