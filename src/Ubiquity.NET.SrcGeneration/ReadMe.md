# Ubiquity.NET.SrcGeneration
This library provides support for source generation using
`System.CodeDom.Compiler.IndentedTextWriter`.
While .NET does have support for T4 to generate source that is used at runtime to generate
a final source this can easily get VERY terse and hard to use. (Let alone debug, especially
with respect to correct white space.) Thus, while it is useful for simpler templating when
things get complicated and there are lots of "decisions" to make based on the input it can
get downright unruly.

## Support includes
* `StringExtensions` to support manipulations of strings commonly used by source generators
    * Method to split a string into lines fit for use in XML doc comments
    * Method to escape processing for a single string for comments
        * Currently the only escape processed is `'\n'` which is transformed into an
          environment specific newline.
    * Method to split a string into lines where a newline is the delimiter
    * Method to Escape a string for use in XML (Specifically XML doc comments but any XML
      usage is valid)
    * Escaping a sequence of strings for use in XML doc comments
    * Sequence of string transforms to remove side by side duplicates
* `IndentedTextWriterExtensions` to provide extensions for an `IndentedTextWriter`
    * Extensions to support auto out-denting via IDisposable in a RAII like pattern
    * Extension to write an empty line (Without any indentation)
        * While `IndentedTextWriter` has this via the `IndentedTextWriter.WriteLineNoTabs(string)`,
          this extension makes it simpler and clearer what the intent was.
    * Extension to generate a block of code that has subsequent content indented.
        * Including an opening and closing text as well as an optional leading line
        ```
        <Leading Line Text>
        <Scope Begin Text>
        [additional lines of text]
        <Scope End - emmitted on Dispose of return (RAII pattern)>
        ```
### C# target language specific support
While other languages are possible this is the only one currently "built-in".
* `CSharpLanguage` contains constants and statics for generating C# source
    * Constants for the open/close of a scope ("{","}")
    * Array of known keywords to allow escaping text that uses them
    * Function to make an identifier out of a string
        * Keywords in the array of known keywords are escaped with a '@' prefix.
* `IndentedTextWriterCsExtensions` To provide extensions to `IndentedTextWriter` that are
  specific to the C# language.
    * Write a `auto-generated` comment in a form recognized by the compiler as `generated`
      for use in determining if analyzers apply or not. (Usually analyzers are set to ignore
      generated code as it isn't something that is controlled by the developer).
    * Write an auto generated comment as a scope to allow adding custom content in the comment
        * Closing of the comment region doesn't occur until the return is Disposed (RAII
          pattern)
    * Write a namespace scope
        * All subsequent content is indented for the scope
        * Closing of the scope doesn't occur until the return is Disposed (RAII pattern).
    * Write a struct scope
        * All subsequent content is indented for the scope
        * Closing of the scope doesn't occur until the return is Disposed (RAII pattern).
    * Write an unsafe block scope
        * All subsequent content is indented for the scope
        * Closing of the scope doesn't occur until the return is Disposed (RAII pattern).
    * Write a generic scope
        * All subsequent content is indented for the scope
        * Closing of the scope doesn't occur until the return is Disposed (RAII pattern).
    * Write a multi-line comment
        * Creates a scope using "\*" and "*/" as the beginning/ending of the block
        * Input string is escaped and converted to a sequence of lines that is emitted
          as the indented contents of the comment.
    * Write a class scope
        * All subsequent content is indented for the scope
        * Closing of the scope doesn't occur until the return is Disposed (RAII pattern).
* `TextWriterCsExtension` to provide more extensions to a `TextWriter` that are specific to
  the C# language.
    * Method to write an attribute line
    * Method to write (without new line) and attribute
    * Method to write an XML Doc comment `summary` tag.
    * Method to write an XML Doc comment `remarks` tag.
    * Method to write an XML Doc comment `summary` and  `remarks` tags with optional default
      `summary` contents
    * Method to write a `using` directive.
