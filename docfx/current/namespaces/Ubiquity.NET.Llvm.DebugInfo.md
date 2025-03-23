---
uid: Ubiquity.NET.Llvm.DebugInfo
remarks: *content
---
This namespace contains all the support for the LLVM representation of debugging information.

## Differences from previous release
A critical difference is that a [Module](xref:Ubiquity.NET.Llvm.Module) does NOT own a [DIBuilder](xref:Ubiquity.NET.Llvm.DebugInfo.DIBuilder)
That was a customized extension that was more accidental as a result of the former releases using object
interning. However, once that was removed it was found that Module instances wereattempting to hold fields
or properties of things that were NOT part of the underlying native object. So, the pattern of use was
changed to better match how the underlying LLVM API worked. In particular a DIBuilder is a `ref struct`
that is ONLY allowed on the stack as it is intended for short term use. It can (and does) own a
[DICompileUnit](xref:Ubiquity.NET.Llvm.DebugInfo.DICompileUnit) and it can reference the module it was
created from. Since it is a `ref struct` it is not allowed to store instances of a DIBuilder in the heap in
any way. ([Kaleidoscope Chapter 9](xref:Kaleidoscope-ch9) provides a sample of use in a visitor pattern where the
instance is provided as an `in` parameter to `ref readonly` functions)
