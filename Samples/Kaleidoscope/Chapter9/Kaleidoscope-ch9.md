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
from the actual format LLVM uses an abstract form of debug data that is based on the common DWARF
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
to track. Thus, for the purposes of this tutorial we'll disable optimizations. (The DisableOptimizations
property of the CodeGenerator was added previously to aid in observing the effects of optimizations and
will serve to disable the optimizations for debugging in this chapter.)

## Setup for emitting debug information
Debug information in Ubiquity.NET.Llvm is created with the [DebugInfoBuilder](xref:Ubiquity.NET.Llvm.DebugInfo.DebugInfoBuilder).
This is similar to the [InstructionBuilder](xref:Ubiquity.NET.Llvm.Instructions.InstructionBuilder). Using the
DebugInfoBuilder requires a bit more knowledge on the general concepts of the DWARF debugging format, and
in particular the [DebuggingMetadata](xref:llvm_sourcelevel_debugging) in LLVM. In Ubiquity.NET.Llvm you don't need
to, and in fact can't, create an instance of the DebugInfoBuilder class. Instead it is lazy constructed
internally to a [BitcodeModule](xref:Ubiquity.NET.Llvm.BitcodeModule) and accessible through the
[DIBuilder](xref:Ubiquity.NET.Llvm.BitcodeModule.DIBuilder) property. This simplifies creating the builder since it
is bound to the module.

Another important item for debug information is called the Compilation Unit. In Ubiquity.NET.Llvm that is the
[DICompileUnit](xref:Ubiquity.NET.Llvm.DebugInfo.DICompileUnit). The compile unit is the top level scope for
storing debug information, there is only ever one per module and generally it represents the full source
file that was used to create the module. Since the compile unit, like the builder is really tied to the
module it is exposed as the [DICompileUnit](xref:Ubiquity.NET.Llvm.BitcodeModule.DICompileUnit) property. However,
unlike a builder it isn't something that a module can automatically construct without more information.
Therefore, Ubiquity.NET.Llvm provides overloads for the creation of a module that includes the additional data
needed to create the DICompileUnit for you.

The updated InitializeModuleAndPassManager() function looks like this:

[!code-csharp[InitializeModuleAndPassManager](CodeGenerator.cs#InitializeModuleAndPassManager)]

There are a few points of interest here. First the compile unit is created for the Kaleidoscope language,
however it is using the [SourceLanguage.C](xref:Ubiquity.NET.Llvm.DebugInfo.SourceLanguage.C) value. This is
because a debugger won't likely understand the Kaleidoscope language, runtime, or calling conventions.
(We just invented it and only now setting up debugger support after all!) The good news is that the
language follows the C language ABI in the code generation (generally a good idea unless you have a really
good reason not to). Therefore, the C language is fairly accurate. This allows calling functions from the
debugger and it will execute them.

Another point to note is that the module ID is derived from the source file path and the source file path
is provided so that it becomes the root compile unit.

>[!IMPORTANT]
> It is important to note that when using the DIBuilder it must be "finalized" in order to resolve internal
> forward references in the debug metadata. The exact details of this aren't generally relevant, just
> remember that somewhere after generating all code and debug information to call the 
> [Finish](xref:Ubiquity.NET.Llvm.DebugInfo.DebugInfoBuilder.Finish(Ubiquity.NET.Llvm.DebugInfo.DISubProgram))
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
compiler for custom languages, including debug support a lot easier.
