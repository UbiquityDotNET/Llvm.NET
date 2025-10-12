# Kaleidoscope Parser
This library implements a Lexer, Parser and AST for the Ubiquity.NET.Llvm Kaleidoscope
tutorial. It currently leverages ANTLR4 to generate a parser and lexer core but the
actual parse technology is abstracted by the AST so it should be possible to use
any parse technology desired. (Though given what ANTLR4 provides it would take a
strong argument to use any other technology).

## Language Syntax
The language syntax follows that of the official LLVM C++ tutorial though, unlike
the C++ version, all versions of the language use the same parser library. To
accomplish this, the grammar takes advantage of the dynamic parsing support found in
ANTLR4 so that various language features are enabled at runtime. This keeps the
individual chapter samples a bit cleaner and focused on the use of
`Ubiquity.NET.Llvm` instead of parsing techniques.

