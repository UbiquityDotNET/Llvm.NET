---
uid: Kaleidoscope-Runtime
---

# Kaleidoscope.Runtime Library
The Kaleidoscope.Runtime Library provides a set of common support libraries to aid in keeping the
tutorial chapter code focused on the code generation and JIT support in Ubiquity.NET.Llvm rather then the
particulars of parsing or the Kaleidoscope language in general.

## REPL Loop infrastructure
The Kaleidoscope.Runtime library contains the basic infrastructure for the classic Read, Evaluate,
Print, Loop (REPL) common for interactive/interpreted/JIT language runtimes. An area in need of some
effort is clean shutdown of the REPL and hosting application. Currently it releies on simply closing
window or a brutal "kill" via Ctrl-C.

### TextReaderExtensions class
The TextReaderExtensions class provides a means to read lines from a TextReader as an `IEnumerable<string>`.
Additionally it supports reading statements from an `IEnumerable<string>` to `IEnumerable<(string Txt, bool IsPartial)>`
the `Txt` member of the tuple is the text from a line of input and the bool `IsPartial` indicates if, the line is
terminated by a semicolon. The extension correctly handles lines with additional text following a semicolon by
splitting the input into multiple lines. Everything up to, and including, the semicolon is yielded with IsPatial
set to false. Then the rest of the line up to the next new line is yielded with IsPartial set to true. (The
implementation supports an arbitrary number of semicolons on the same line and yields complete statements for
each one)

## Parser
Kaleidoscope for Ubiquity.NET.Llvm leverages ANTLR4 for parsing the language into a parse tree. Since ANTRL is a
generalized parsing infrastructure it has a lot of flexibility depending on the needs of a language and
application. While that flexibility is generally a value, it does come with added complexity. The ParserStack
class encapsulates the complexity as needed for the Kaleidoscope language code generation to minimize
the repetition of boiler-plate code, and in particular the correct set of listeners for errors and user
operators. This stack supports two distinct modes of parsing, interactive REPL, and FULL source as used
for ahead of time compilation.

## Kaleidoscope JIT engine
The JIT engine used for Kaleidoscope is based on the Ubiquity.NET.Llvm OrcJit, which, unsurprisingly, uses the LLVM
OrcJit functionality to provide On Request Compilation (ORC). For most of the chapters, the JIT uses a
moderately lazy compilation technique where the source language is parsed, converted to LLVM IR and submitted
to the JIT engine. The JIT engine does not immediately generate native code from the module, however. Instead
it stores the module, and whenever compiled code calls to a symbol exported by the IR module, it will then
generate the native code for the function "on the fly". This has the advantage of not paying the price of
converting IR to native code if it is never actually used, though it does have the cost of converting the
source language to IR, even if the code will never execute.

### Really lazy compilation
While the basic lazy compilation of IR to native code has performance benefits over a pure interpreter, it
still has the potential for wasted overhead converting the parsed language to LLVM IR. Fortunately, the LLVM
and Ubiquity.NET.Llvm OrcJit support truly lazy compilation. This is done by asking the JIT to create a stub for
a named symbol and then, whenever code calls that symbol the stub calls back to the JIT which then calls back
the application to generate the IR, add the module to the JIT and trigger compilation to native. Thus, achieving
true Just-In-Time compilation.

>[!CAUTION]
> The truly lazy functionality is currently disabled due to a bug in the underlying LLVM OrcJIT
> implementation on Windows.
