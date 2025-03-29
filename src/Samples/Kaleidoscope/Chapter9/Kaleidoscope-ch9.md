---
uid: Kaleidoscope-ch9
---

# 9. Kaleidoscope: Adding Debug Information
So far in the progress of the Kaleidoscope tutorials we've covered the basics of the language as a JIT
engine and even added ahead of time compilation into the mix so it is a full static compiled language. But
what happens if something goes wrong in one of the programs written in Kaleidoscope? How can a developer
debug applications written in this wonderful new language? Up until now, the answer is, you can't. This
chapter will add debugging information to the generated object file so that it is available for debuggers.

Source level debugging uses formatted data bound into the output binaries that helps the debugger map the
state of the application to the original source code that created it. The exact format of the data depends
on the target platform but the general idea holds for all of them. In order to isolate front-end developers
from the actual format - LLVM uses an abstract form of debug data that is based on the common DWARF
debugging format. Internally, the LLVM target will transform the abstract representation into the actual
target binary form.

>[!NOTE]
> Debugging JIT code is rather complex as it requires awareness of the runtime within the debugger to
> manage the execution and runtime state etc... Such functionality is beyond the scope of this tutorial.

## Why is it a hard problem?
Debugging is a tough problem for a number of reasons, mostly revolving around optimized code. Optimizations
make keeping source level information more difficult. In LLVM the original source location information is
attached to each LLVM IR instruction. Optimization passes should keep the source location for any new
instructions created, but merged instructions only get to keep a single source location. This is generally
the cause of the observed "jumping around" when debugging optimized code. Additionally, optimizations can
move variables in ways that are either optimized out, shared in memory, in registers or otherwise difficult
to track. Thus, for the purposes of this tutorial we'll skip optimizations.

## Setup for emitting debug information
Debug information in Ubiquity.NET.Llvm is created with the [DIBuilder](xref:Ubiquity.NET.Llvm.DebugInfo.DIBuilder).
This is similar to the [InstructionBuilder](xref:Ubiquity.NET.Llvm.Instructions.InstructionBuilder). Using the
DIBuilder requires a bit more knowledge on the general concepts of the DWARF debugging format, and
in particular the [DebuggingMetadata](xref:llvm_sourcelevel_debugging) in LLVM. In Ubiquity.NET.Llvm you need
to, create an instance of the DIBuilder class bound to a particular module. Such a builder is disposable and
therefore requires a call to Dispose(). Normally this is handled in a `using` expression.

Another important item for debug information is called the Compilation Unit. In Ubiquity.NET.Llvm that is the
[DICompileUnit](xref:Ubiquity.NET.Llvm.DebugInfo.DICompileUnit). The compile unit is the top level scope for
storing debug information generally it represents the full source file that was used to create the module.
(Though with IR linking it is plausible that a module has multiple Compile Units associated).
Unlike a builder it isn't something that is constructed without more information.
Therefore, Ubiquity.NET.Llvm provides overloads for the creation of a module that includes the additional data
needed to create the DICompileUnit for you. It is important to note that a DIBuilder may have ONLY one
DICompileUnit and that unit is used for all of the debug nodes it builds. It must be set when finalizing
the debug information in order to properly resolve items to the compilation unit.

TODO: Discuss DIBuilder as a ref struct and that it must be passed through as part of the "visitor"

Another point to note is that the module ID is derived from the source file path and the source file path
is provided so that it becomes the root compile unit.

>[!IMPORTANT]
> It is important to note that when using the DIBuilder it must be "finalized" in order to resolve internal
> forward references in the debug metadata. The exact details of this aren't generally relevant, just
> remember that somewhere after generating all code and debug information to call the 
> [Finish](xref:Ubiquity.NET.Llvm.DebugInfo.DIBuilder.Finish(Ubiquity.NET.Llvm.DebugInfo.DISubProgram))
> method. (In Ubiquity.NET.Llvm this method is called Finish() to avoid conflicts with the .NET runtime defined
> Finalize() and to avoid confusion on the term as the idea of "finalization" has a very different meaning
> in .NET then what applies to the DIBuilder).

The tutorial takes care of finishing the debug information in the generator's Generate method after
completing code generation for the module.

[!code-csharp[Generate](CodeGenerator.cs#Generate)]

## Functions
With the basics of the DIBuilder and DICompile unit setup for the module it is time to focus on providing
debug information for functions. This requires adding a few extra calls to build the context of the debug
information for the function. The DWARF debug format that LLVM's debug metadata is based on calls these
"SubPrograms". The new code builds a representation of the file the code is contained in as a new 
[DIFile](xref:Ubiquity.NET.Llvm.DebugInfo.DIFile). In this case that is a bit redundant as all the code comes from
a single file and the compile unit already has the file info. However, that's not always true for all
languages (i.e. some sort of Include mechanism) so the file is created. It's not a problem as LLVM will
intern the file definition so that it won't actually end up with duplicates.


[!code-csharp[GetIrDeclareFunction](CodeGenerator.cs#GetOrDeclareFunction)]

## Debug Locations
The AST contains full location information for each parsed node from the parse tree. This allows building
debug location information for each node fairly easily. The general idea is to set the location in the
InstructionBuilder so that it is applied to all instructions emitted until it is changed. This saves on
manually adding the location on every instruction.

[!code-csharp[EmitLocation](CodeGenerator.cs#EmitLocation)]

## Function Definition
The next step is to update the function definition with attached debug information. The definition starts
by pushing a new lexical scope that is the functions declaration. This serves as the parent scope for all
the debug information generated for the function's implementation. The debug location info is cleared from
the builder to set up all the parameter variables with alloca, as before. Additionally, the debug
information for each parameter is constructed. After the function is fully generated the debug information
for the function is finalized, this is needed to allow for any optimizations to occur at the function
level.

[!code-csharp[DefineFunction](CodeGenerator.cs#FunctionDefinition)]

## Debug info for Parameters and Local Variables
Debug information for parameters and local variables is similar but not quite identical. Thus, two new
overloaded helper methods `AddDebugInfoForAlloca` handle attaching the correct debug information for
parameters and local variables.

[!code-csharp[CreateEntryBlockAlloca](CodeGenerator.cs#AddDebugInfoForAlloca)]

## Conclusion
Adding debugging information in LLVM IR is rather straight forward. The bulk of the problem is in tracking
the source location information in the parser. Fortunately for Ubiquity.NET.Llvm version of Kaleidoscope, the ANTLR4
generated parsers do this for us already! Thus, combining the parser with Ubiquity.NET.Llvm makes building a full
compiler for custom languages, including debug support a lot easier. The most "complex" part is handling the
correct ownership semantics for a DIBuilder but that is generally enforced by the compiler as it is a 
`ref struct` type.
