---
uid: Ubiquity.NET.Llvm.Transforms.Legacy
remarks: *content
---
This namespace contains the wrappers for supporting the "legacy" pass management. This is
***NOT*** normally used by applications as the new pass manager support is built into
[Module](xref:Ubiquity.NET.Llvm.Module) and [Function](xref:Ubiquity.NET.Llvm.Values.Function)
via one of the overloads of `TryRunPasses(...)`. Generally the legacy pass manager support
is only used for final target code generation and not exposed for LLVM-C consumption and
therefore not of any real use.
