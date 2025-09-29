---
uid: Kaleidoscope-ch4
---

# 4. Kaleidoscope: Adding JIT
At this point things generally re-converge with the official LLVM tutorials
(optimization was already covered in the previous sub-chapter.)

## Adding JIT Compilation
Now that the code generation produces optimized code, it is time to get to the fun
part - executing code! The basic idea is to allow the user to type in the
Kaleidoscope code as supported thus far and it will execute to produce a result.
Unlike the previous chapters, instead of just printing out the LLVM IR
representation of a top level expression this sample will execute the code and
provide the results back to the user! (Like a real language/Tool should!)

### Main Driver
The changes needed to the main driver are pretty simple, mostly consisting of
removing a couple lines of code that print out the LLVM IR for the module at the
end when defined. The code already supported showing the results if it was a
floating point value by checking if the generated value is a
[ConstantFP](xref:Ubiquity.NET.Llvm.Values.ConstantFP). We'll see a bit later on
why that is a ConstantFP value.

### Code Generator
The code generation needs an update to support using a JIT engine to generate and
execute the Kaleidoscope code provided by the user.

#### Generator fields
To begin with, the generator needs some additional members, including the JIT
engine.

[!code-csharp[PrivateMembers](CodeGenerator.cs#PrivateMembers)]

The JIT engine is retained for the generator to use. The same engine is held for
the lifetime of the generator so that functions are added to the same engine and
can call functions previously added. The JIT provides a 'tracker' for every module
added, which is used to reference the module in the JIT, this is normally used to
remove the module from the JIT engine when re-defining a function. Thus, a map of
the function names and the JIT tracker created for them is maintained. Additionally,
a collection of defined function prototypes is retained to enable matching a
function call to a previously defined function. Since the JIT support uses a module
per function approach, lookups on the current module aren't sufficient.

The JIT engine use a [ThreadSafeContext](xref:Ubiquity.NET.Llvm.OrcJITv2.ThreadSafeContext)
and [ThreadSafeModule](xref:Ubiquity.NET.Llvm.OrcJITv2.ThreadSafeModule) to manage
callbacks and materialization in the JIT while supporting multiple threads of
execution. Thus the context type for all modules and generation options needs the
new type.

As described previously the names of functions the module is generated for is held
in a dictionary with the [ResourceTracker](xref:Ubiquity.NET.Llvm.OrcJITv2.ResourceTracker)
for that module to ensure it is 'removable'.

#### Generator initialization
The initialization of the generator requires updating to support the new members.

[!code-csharp[Initialization](CodeGenerator.cs#Initialization)]

In particular, the static output writer is set for the JIT to use whatever writer
was provided. Normally, this is the system console but for testing it can be any
standard `TextWriter`. Then the [ThreadSafeContext](xref:Ubiquity.NET.Llvm.OrcJITv2.ThreadSafeContext)
is created for the generator and used to create the instruction builder.

#### JIT Engine
The JIT engine itself is a class provided in the Kaleidoscope.Runtime library
that wraps a Ubiquity.NET.Llvm OrcJIT engine. It is NOT derived from that class as
a JIT engine is created using a "Builder" or factory pattern. So it is not possible
to create a derived type using a builder.

[!code-csharp[Kaleidoscope JIT](../../../Samples/Kaleidoscope/Kaleidoscope.Runtime/KaleidoscopeJIT.cs)]

[LLJit](xref:Ubiquity.NET.Llvm.OrcJITv2.LLJit) provides support for declaring
functions that are external to the JIT that the JIT'd module code can call (
Absolutes). For Kaleidoscope, two such functions are defined directly in
`KaleidoscopeJIT` (`putchard` and `printd`), which is consistent with the same 
functions used in the official LLVM C++ tutorial. Thus, allowing sharing of samples
between the two. These functions are used to provide rudimentary console output
support.

> [!WARNING]
> All such methods implemented in .NET must block any exception from bubbling out
> of the call as the JIT engine doesn't know anything about them and neither does
> the Kaleidoscope language. Exceptions thrown in these functions would produce
> undefined results, at best - probably crashing the application.


#### Generator Dispose
Since the JIT engine is disposable, the code generators Dispose() method must now
call the Dispose() method on the JIT engine.

[!code-csharp[Dispose](CodeGenerator.cs#Dispose)]

#### Generate Method
To actually execute the code the generated modules are added to the JIT. If the
function is an anonymous top level expression, it is eagerly compiled and a
delegate is retrieved from the JIT to allow calling the compiled function directly.
The delegate is then called to get the result. Once an anonymous function produces
a value, it is no longer used so is removed from the JIT and the result value
returned. For other functions the module is added to the JIT and the function is
returned.

For named function definitions, the module is lazy added to the JIT as it isn't
known if/when the function is called. The JIT engine will compile modules lazy
added into native code on first use. Though, if the function is never used, then
creating the IR module was wasted. ([Chapter 7.1](xref:Kaleidoscope-ch7.1) has a
solution for even that extra overhead - truly lazy JIT). Since Kaleidoscope is
generally a dynamic language it is possible and reasonable for the user to
re-define a function (to fix an error, or provide a completely different
implementation all together). Therefore, any named functions are removed from the
JIT, if they existed, before adding in the new definition. Otherwise the JIT
resolver would still resolve to the previously compiled instance.

[!code-csharp[Generate](CodeGenerator.cs#Generate)]

Keeping all the JIT interaction in the generate method isolates the rest of the
generation from any awareness of the JIT. This will help when adding truly lazy JIT
compilation in [Chapter 7.1](xref:Kaleidoscope-ch7.1) and AOT compilation in
[Chapter 8](xref:Kaleidoscope-ch8)

#### GetOrDeclareFunction()
Next is to update the GetOrDeclareFunction() to handle the new support for
[ThreadSafeContext](xref:Ubiquity.NET.Llvm.OrcJITv2.ThreadSafeContext) and a sanity
check for the nullability of a module. 

[!code-csharp[Main](CodeGenerator.cs#GetOrDeclareFunction)]

#### Function Definitions
Visiting a function definition needs to remove the previously added manual step of
running the optimization passes. That is now handled by the Kaleidoscope JIT as a
transformation layer. Before final target code generation is performed on a symbol
the transforms are run to perform any modifications desired. This makes the
optimization process a lazy operation as well as the final target machine native
code generation. The JIT is setup with a default pass pipeline that is roughly
equivalent to a the Clang compiler with 'O3' optimizations.

[!code-csharp[Main](CodeGenerator.cs#FunctionDefinition)]

## Conclusion
While the amount of words needed to describe the changes to support JIT execution
here isn't exactly small, the actual code changes required really are. The Parser
and JIT engine do all the heavy lifting. Ubiquity.NET.Llvm.JIT provides a clean
interface to the underlying LLVM OrcJIT v2 that fits with common patterns and
runtime support for .NET. Very cool, indeed! :nerd_face:
