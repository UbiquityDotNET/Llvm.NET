---
uid: Kaleidoscope-ch3.5
---

# 3.5 Kaleidoscope: Generating LLVM IR With optimizations
This chapter focuses on the basics of optimization with LLVM IR. The general goal is
to parse Kaleidoscope source code to generate a [Module](xref:Ubiquity.NET.Llvm.Module)
representing the source as optimized LLVM IR. This is broken out as a distinct chapter to help
identify the support for profiling and how it is different from the LLVM source samples that
link directly to the LLVM libraries (That is, the samples are written in C++ AND continue to use
the now VERY legacy optimization pass management support. [It is so legacy now that almost ALL remnants
of it are removed from the LLVM-C API, not just dpeprecated])

## Code generation
The Core of this sample doesn't change much from [Chapter 3](xref:Kaleidoscope-ch3). It simply adds
module generation with optimized IR. To do that there are a few changes to make. In fact the optimizations
provided don't do much and the resulting IR is much the same.  
[Coming up with a more complex Kaleidoscope
sample that actually uses the optimizations more is left as an excercise for the reader. :wink: ]

### Initialization
The code generation maintains state for the transformation as private members. To support optimization
generally only requires a set of named passes and to call the method to run the passes on a function or
module. [Technically an overlod provides the chance to set [PassBuilderOptions](xref:Ubiquity.NET.Llvm.PassBuilderOptions) but
this sample just uses the overload that applies defaults.] The new pass management system
uses the string names of passes instead of a distinct type and named methods for adding them etc...

These Options are initialized in a private static member for the passes.
[!code-csharp[Main](CodeGenerator.cs#PrivateMembers)]

### Special attributes for parsed functions
When performing optimizations with the new pass builder system the TargetLibraryInfo (Internal LLVM concept) is
used to determine what the "built-in" functions are. Unfortunately they leave little room for manipulating or
customizing this set (In C++, in LLVM-C there is NO support for this type at all!). Unfortunately that means that
if any function happens to have the same name as the TargetLibraryInfo for a given Triple then it will be optimized
AS a built-in function (even if not declared as one). This is an unfortunate state of affairs with the LLVM support
for C++ and highly problematic for `C` based bindings/projections like this library. Fortunately there is a scapegoat
for this. The function can include a `nobuiltin` attribute to prevent the optimizer from assuming calls to it are
one of the well known built-in functions. This is used for ALL methods that come from the AST, which is all functions
at this point in the language design. Thus `GetOrDeclareFunction` will add that attribute for any function creations.

[!code-csharp[Main](CodeGenerator.cs#GetOrDeclareFunction)]


### Function Definition
The only other major change for optimization support is to actually run the optimizations. In LLVM optimizations
are supported at the module or individual function level. For this sample each function definition is optimised
as each is returned individually. That will change in later chapters. Thus the only real change is after generating
a new function for a given AST definition the optimization passes are run for it. This involves calling one of the
overloads of the `TryRunPasses` function and then checking for errors.

[!code-csharp[Main](CodeGenerator.cs#FunctionDefinition)]

