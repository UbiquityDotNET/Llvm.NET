# Kaleidoscope Parse Tree Visitors
The LLVM.NET Kaleidoscope tutorials all use ANTLR4 to parse the Kaleidoscope language When ANTLR processes the
language grammar description to generate the lexer and parser it also generates a base "visitor" class for applying
the [Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern) to the parse tree. This is the primary mechanism
for generating code in these examples. All of the CodeGenerator classes derive from the ANTLR generated base visitor class.

There are only a few top level rules in the grammar to consider (although each has many sub rules).

[!code-antlr[repl](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/Kaleidoscope.g4?start=181&end=187)]

The parse tree consist of a tree of nodes generated for each rule in the grammar. The rule classes are generated at
build time when the antlr grammar file is parsed. The C# target for ANTLR4 will generate the rule classes with the
partial keyword, allowing the application code to add application specific support to the rule classes, and thus to
the parse tree. For Kaleidoscope, this is used to provide simpler access to the information parsed from the Kaleidoscope
language input and effectively removes the need for an AST. (e.g. The parse tree is the AST so an intermediate transform
isn't needed). For simplicity and clarity of understanding each of the extended partial classes are placed into their
own source file.

The code generation process involves walking the tree to produce the final output. Each chapter uses a different generator
implementation but they are all implementations of a parse tree visitor.
