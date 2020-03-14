# Kaleidoscope Parser
This library implements a Lexer/Parser for the LUbiquity.NET.Llvm Kaleidoscope tutorial.
The language syntax follows that of the official LLVM C++ tutorial though, unlike
the C++ version, all versions of the language (Chapters 2-7) use the same parser
library. To accomplish this, the grammar takes advantage of the dynamic parsing
support found in ANTLR4 so that various language features are enabled at runtime.
This keeps the individual chapter samples a bit cleaner and focused on the use of
Ubiquity.NET.Llvm instead of parsing. 

