---
uid: Kaleidoscope-Runtime
---

# Kaleidoscope.Runtime Library
The Kaleidoscope.Runtime Library provides a set of common support libraries to aid in
keeping the tutorial chapter code focused on the code generation and JIT support in
`Ubiquity.NET.Llvm` rather then the particulars of the Kaleidoscope language in general. It
serves as a useful reference for the implementation of other custom DSLs. (Along with a core
part of the sample infrastructure of this repository)

## Kaleidoscope specific REPL Loop support
The Kaleidoscope.Runtime library contains a language/runtime specific implementation of the
classic Read, Evaluate, Print, Loop (REPL) common for interactive/interpreted/JIT language
run-times. This uses an asynchronous pattern and allows cancellation via a standard
cancellation token. This supports a clean shutdown via a CTRl-C handler etc...

## Kaleidoscope JIT engine
The JIT engine used for Kaleidoscope is based on the Ubiquity.NET.Llvm OrcJIT v2, which,
unsurprisingly, uses the LLVM OrcJit functionality to provide On Request Compilation (ORC).
For most of the chapters, the JIT uses a moderately lazy compilation technique where the
source language is parsed, converted to LLVM IR and submitted to the JIT engine. The JIT
engine does not immediately generate native code from the module, however. Instead it stores
the module, and whenever compiled code calls to a symbol exported by the IR module, it will
then generate the native code for the function "on the fly". This has the advantage of not
paying the price of converting IR to native code if it is never actually used, though it
does have the cost of converting the source language to IR, even if the code will never
execute.

### Really lazy compilation
While the basic lazy compilation of IR to native code has performance benefits over a pure
interpreter, it still has the potential for wasted overhead converting the parsed language
to LLVM IR. Fortunately, the LLVM and Ubiquity.NET.Llvm.OrcJitv2 supports truly lazy
compilation. This is done by asking the JIT to create a stub for a named symbol and then,
whenever code calls that symbol the stub calls back to the JIT which then calls back the
application to 'materialize' the IR, add the module to the JIT and trigger compilation to
native. Thus, achieving true Just-In-Time compilation.
