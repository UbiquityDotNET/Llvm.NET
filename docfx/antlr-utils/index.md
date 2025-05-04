# About
This library provides general extensions to ANTLR including adapter bindings
for the Ubiquity.NET.Runtime library.

# Key usage
* Get a SourceLocation from various ANTLR types (rule,tokens,terminals)
    - This provides an adaptation to the abstract SourceLocation
* Debug trace listener
    - Provides debug TRACE support for any parser by listening for every rule and using
      Debug.Trace() to generate a string representation of that rule. This is VERY useful
      when developing or debugging a grammar.
* Adapter for parse error listeners to a unified and abstract
  `Ubiquity.NET.Runtime.IParseErrorListener`.
  - This allows building consumers that deal with errors and remain indepent of the parsing
    technology.
* Extension functions that provides commonly used support for ANTLR
    -  Get a character interval from a ParserRuleContext with support for the standard EOF
       rule.
    - Get the source stream from an IRecognizer
    - Get the source text from a rule context and recognizer that produced it.
    - Get source text from a rule context and stream that it was parsed from.
    - Get a unique ID for a parse tree
        * Useful for building graphs of the result of parsing as many graphing representations
          require a unique node id for every node in the graph.
