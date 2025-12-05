# Ubiquity.NET
Ubiquity.NET family of libraries provides support for a number of scenarios but the primary
focus is AOT code generation of .NET for Embedded systems. We aren't quite there yet, but
are rather close. In the mean time this set of libraries provides the building blocks needed
for creating a Domain Specific Language (DSL) implementation or custom language compiler,
including JIT execution.

## The Libraries[<sup>1</sup>](#footnote_1) in this repository

| Library | Description |
|---------|-------------|
| [Ubiquity.NET.Llvm](llvm/index.md) | This library contains The core of the LLVM projection to .NET |

## Analyzers [<sup>2</sup>](#footnote_2)
| Library | Description |
|---------|-------------|
| [Ubiquity.NET.Llvm.Analyzer](Analyzers/index.md) | This analyzer helps identify ambiguities with regard to equality |

---
<a id="footnote_1"/><sup>1</sup> The Ubiquity.NET.Llvm.Interop is intentionally NOT
documented. It is an internal implementation detail subject to change in the future. There
are plans to merge it with the OO wrapper library. Therefore, applications should NOT depend
on it as it is likely to cease existing in the future.

<a id="footnote_1"/><sup>2</sup> The analyzer is included in the `Ubiquity.NET.Llvm` package.
