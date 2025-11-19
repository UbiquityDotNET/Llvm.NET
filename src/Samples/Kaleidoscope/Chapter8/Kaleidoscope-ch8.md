---
uid: Kaleidoscope-ch8
---

# 8. Kaleidoscope: Compiling to Object Code
This tutorial describes how to adapt the Kaleidoscope JIT engine into an Ahead of Time (AOT)
compiler by generating target specific native object files.

## Choosing a target
LLVM has built-in support for cross-compilation. This allows compiling to the architecture
of the platform you run the compiler on or, just as easily, for some other architecture. For
the Kaleidoscope tutorial we'll focus on just the native target the compiler is running on.

LLVM uses a "Triple" string to describe the target used for code generation. This takes the
form `<arch><sub>-<vendor>-<sys>-<abi>` (see the description of the [Triple](xref:Ubiquity.NET.Llvm.Triple)
type for more details)

Fortunately, it is normally not required to build such strings directly.

## Grammar
In the preceding chapters the Kaleidoscope implementation provided an interactive JIT based
on the classic Read Evaluate Print Loop (REPL). So the grammar focused on a top level rule
"repl" that processes individual expressions one at a time. For native compilation this
complicates the process of parsing and processing a complete file. To handle these two
distinct scenarios the grammar has different rules. For the interactive scenario the
previously mentioned "repl" rule is used. When parsing a full source file the "fullsrc" rule
is used as the start.

``` antlr
// Full source parse accepts a series of definitions or prototypes, all top level expressions
// are generated into a single function called Main()
fullsrc
    : repl*;
```

This rule simply accepts any number of expressions so that a single source file is parsed to
a single complete parse tree. (This particular point will become even more valuable when
generating debug information in [Chapter 9](xref:Kaleidoscope-ch9) as the parse tree nodes
contain the source location information based on the original input stream).

## Code Generation Changes
The changes in code generation are fairly straight forward and consist of the following
basic steps.
1. Remove JIT engine support
2. Expose the bit code module generated, so it is available to the "driver".
3. Saving the target machine (since it doesn't come from the JIT anymore)
4. Keep track of all generated top level anonymous expressions
5. Once generating from the parse tree is complete generate a main() that includes calls to
   all the previously generated anonymous expressions.

Most of these steps are pretty straight forward. The anonymous function handling is a bit
distinct. Since the language syntax allows anonymous expressions throughout the source file,
and they don't actually execute during generation - they need to be organized into an
executable form. Thus, a new list of the generated functions is maintained and, after the
tree is generated, a new main() function is created and a call to each anonymous expression
is made with a second call to printd() to show the results - just like they would appear if
typed in an interactive console. A trick used in the code generation is to mark each of the
anonymous functions as private and always inline so that a simple optimization pass can
eliminate the anonymous functions after inlining them all into the main() function.

``` C#
// mark anonymous functions as always-inline and private so they can be removed
if(definition.IsAnonymous)
{
    function.AddAttribute( FunctionAttributeIndex.Function, "alwaysinline"u8 )
            .Linkage( Linkage.Private );

    AnonymousFunctions.Add( function );
}
```

These settings are leveraged after generating from the tree to create the main function. A
simple loop generates a call to each expression along with the call to print the results.

> [!NOTE]
> The always inliner will inline the functions marked as inline and the dead code
> elimination pass will eliminate unused internal/private global symbols. This has the
> effect of generating the main function with all top level expressions inlined and the
> originally generated anonymous functions removed.

[!code-csharp[Generate](CodeGenerator.cs#Generate)]

Most of the rest of the changes are pretty straightforward following the steps listed
previously.

### Anonymous Function Definitions
As previously mentioned, when generating the top level expression the resulting function is
added to the list of anonymous functions to generate a call to it from main().

[!code-csharp[FunctionDefinition](CodeGenerator.cs#FunctionDefinition)]


## Driver changes
To support generating object files the "driver" application code needs some alterations. The
changes fall into two general categories:

1. Command line argument handling
2. Generating the output files

### Adding Command Line handling
To allow providing a file like a traditional compiler the driver app needs to have some
basic command line argument handling. ("Basic" in this case means truly rudimentary :grin: )
Generally this just gets a viable file path to use for the source code.

[!code-csharp[ProcessArgs](Program.cs#ProcessArgs)]

### Update Main()
The real work comes in the Main application driver, though there isn't a lot of additional
code here either. The general plan is:
1. Process the arguments to get the path to compile
2. Open the file for reading
3. Create a new target machine from the default triple of the host
4. Create the parser stack
5. Parse the input file
6. Generate the IR code from the parse tree
7. Once the parsing has completed, verify the module and emit the object file
8. For diagnostics use, also emit the LLVM IR textual form and assembly files

[!code-csharp[Main](Program.cs#Main)]

## Conclusion
That's it - seriously! Very little change was needed, mostly deleting code and adding the
special handling of the anonymous expressions. Looking at the changes it should be clear
that it is possible to support runtime choice between JIT and full native compilation
instead of deleting the JIT code. (Implementing this feature is "left as an exercise for the
reader" :wink:)




