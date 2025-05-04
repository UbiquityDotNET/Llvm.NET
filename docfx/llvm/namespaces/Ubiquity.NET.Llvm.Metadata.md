---
uid: Ubiquity.NET.Llvm.Metadata
remarks: *content
---
This namespace hosts all of the wrappers for the LLVM Metadata. The namespace contains the root
of the metadata type system [IrMetadata](xref:Ubiquity.NET.Llvm.Metadata.IrMetadata).

>[NOTE]
> The name [IrMetadata](xref:Ubiquity.NET.Llvm.Metadata.IrMetadata) is used to help deal with a
> number of naming issues and conflicts with existing types or namespaces.
> ([CA1724](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1724)
> is not your friend on this one. The name `Metadata` conflicts with namespace
> [System.Runtime.Remoting.Metadata](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.remoting.metadata?view=netframework-4.8.1),
> which apparently is ONLY a legacy desktop framework namespace, but the tooling still complains
> about a conflict between the namespace name and a type name). To resolve this the type is named
> as closely as possible without causing the conflict.
