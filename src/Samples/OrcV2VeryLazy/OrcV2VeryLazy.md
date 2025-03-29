---
uid: orcjitv2-very-lazy
---
# ORC JIT v2 Very Lazy sample
This sample is based on the official LLVM C sample but adapted to demonstrate the use of the
Ubiquity.NET.llvm libraries. The sample builds a basic function that calls to an unresolved
function. The unresolved function body is materialized through an delegate that will parse
the LLVM IR for the mody to produce the required module. It then "emits" that module to the
JIT engine before returning. This demonstrates how lazy JIT symbol resolution and materializers
operate to allow use with any source. In this sample the source is just LLVM IR in textual form
but that is not a requirement. (It helps to keep the sample simple without crowding it with
parsing and other language specific cruft.)
