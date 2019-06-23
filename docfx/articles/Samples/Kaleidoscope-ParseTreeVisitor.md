# Kaleidoscope Parse Tree Visitors
The LLVM.NET Kaleidoscope tutorials all use ANTLR4 to parse the Kaleidoscope language When ANTLR processes the
language grammar description to generate the lexer and parser it also generates a base "visitor" class for applying
the [Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern) to the parse tree. This is the primary mechanism
for generating the [AST](Kaleidoscope-AST.md) for these examples. The AST transformation classes derive from the ANTLR
generated base visitor class.

There are only a few top level rules in the grammar to consider (although each has many sub rules).

[!code-antlr[repl](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/Kaleidoscope.g4?start=181&end=187)]

The parse tree consist of a tree of nodes generated for each rule in the grammar. The rule classes are generated at
build time when the antlr grammar file is parsed. The C# target for ANTLR4 will generate the rule classes with the
partial keyword, allowing the application code to add application specific support to the rule classes, and thus to
the parse tree. For Kaleidoscope, this is used to provide simpler access to the information parsed from the Kaleidoscope
language input simplifying generation of the AST. For simplicity and clarity of understanding each of the extended partial
classes are placed into their own source file.

Generally speaking, the use of ANTLR and the ParseTree is a hidden internal implementation detail of the Llvm.NET
Kaleidoscope tutorials. The actual code generation deals only with the AST so the parsing could be done with some
other technology. (Though with all the functionality that ANTLR4 provides it would take a strong argument to justify
something else)
