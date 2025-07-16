# Ubiquity.NET
Ubiquity.NET family of libraries provides support for a number of scenarios but the primary focus is
AOT code generation of .NET for Embedded systems. We aren't quite there yet, but are rather close. In
the mean time this set of libraries provides the building blocks needed for creating a Domain Specific
Language (DSL) implementation including JIT execution. Several useful generalized libraries are also
included.

## The Libraries in this repository
(At least the ones generating docs at this point anyway! :grin:)

| Library | Description |
|---------|-------------|
| [Ubiquity.NET.Llvm](llvm/index.md) | This library contains The core of the LLVM projection to .NET |
| [Ubiquity.NET.Runtime.Utils](runtime-utils/index.md) | This library contains common support for DSL runtime and language implementors |
| [Ubiquity.NET.Extensions](extensions/index.md) | This library contains general extensions and helpers for many scenarios using .NET |
| [Ubiquity.NET.Antlr.Utils](antlr-utils/index.md) | This library contains extensions and helpers for using ANTLR with .NET |
| [Ubiquity.NET.InteropHelpers](interop-helpers/index.md) | This library contains extensions and helpers for implementing interop support for native libraries |
