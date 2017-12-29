# Chapter 3 - Code Generation to LLVM IR
This chapter focuses on the basics of transforming the ANTLR parse tree into LLVM IR. The general goal is to
parse Kaleidoscope source code to generate a [BitcodeModule](xref:Llvm.NET.BitcodeModule) representing the
source as LLVM IR.

## Parse Tree Visitors
When ANTLR processes the language grammar description to generate the lexer and parser it also generates a
base "visitor" class for applying the [Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern) to
the parse tree. This is the primary mechanism for generating code in these examples. All of the CodeGenerator
classes derive from the ANTLR generated base visitor class.

There are only a few top level rules in the grammar to consider (although each has many sub rules).

[!code-antlr[repl](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/Kaleidoscope.g4?start=181&end=187)]

