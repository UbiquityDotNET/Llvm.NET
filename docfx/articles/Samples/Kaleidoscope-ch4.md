# 4. Kaleidoscope: Adding JIT and Optimizer Support
This chapter of the Kaleidoscope tutorial introduces Just-In-Time (JIT) compilation and simple optimizations
of the generated code. As, such this is the first variant of the language implementation where you can actually
execute the Kaleidoscope code. Thus, this is a bit more fun than the others as you finally get to see the
language working for real!

## Constant Folding
If you studied the LLVM IR generated from the previous chapters you will see that it isn't particularly
well optimized. There is one case, though, where it does do some nice optimization automatically for us.

For example:
```Kaleidoscope
def test(x) 1+2+x;
```

produces the following LLVM IR:
```llvm
define double @test(double %x) {
entry:
  %addtmp = fadd double 3.000000e+00, %x
  ret double %addtmp
}
```

That's not exactly what the parse tree would suggest. The [InstructionBuilder](xref:Llvm.NET.Instructions.InstructionBuilder)
automatically performs an optimization technique known as 'Constant Folding'. This optimization is very
important, in fact, many compilers implement the folding directly into the generation of the Abstract
Syntax Tree (AST). With LLVM, that isn't necessary as it is automatically provided for you (no extra
charge!).

Obviously constant folding isn't the only possible optimization and InstructionBuilder only operates on
the individual instructions as they are built. So, there are limits on what InstructionBuilder can do.

For example:

```Kaleidoscope
def test(x) (1+2+x)*(x+(1+2));
```

```llvm
define double @test(double %x) {
entry:
  %addtmp = fadd double 3.000000e+00, %x
  %addtmp1 = fadd double %x, 3.000000e+00
  %multmp = fmul double %addtmp, %addtmp1
  ret double %multmp
}
```

In this case the operand of the additions are identical. Ideally this would generate as
`temp = x+3; result = temp*temp;` rather than computing X+3 twice. This isn't something that
InstructionBuilder alone can do. Ultimately this requires two distinct transformations:
  1. Re-association of expressions to make the additions lexically identical (e.g. recognize that x+3 == 3+x )
  1. Common Subexpression Elimination to remove the redundant add instruction.

Fortunately, LLVM provides a very broad set of optimization transformations that can handle this and many
other scenarios.

## LLVM Optimization Passes
LLVM provides many different optimization passes, each handling a specific scenario with different trade-offs.
One of the values of LLVM as a general compilation back-end is that it doesn't enforce any particular set
of optimizations. By default, there aren't any optimizations (Other than the obvious constant folding built
into the InstructionBuilder). All optimizations are entirely in the hands of the front-end application.
The compiler implementor controls what passes are applied, and in what order they are run. This ensures
that the optimizations are tailored to correctly meet the needs of the language and runtime environment.

For Kaleidoscope, optimizations are limited to a single function as they are generated when the user types
them in on the command line. Ultimate, whole program optimization is off the table (You never know when the
user will enter the last expression so it is incorrect to eliminate unused functions). In order to support
per-function optimization a [FunctionPassManager](xref:Llvm.NET.Transforms.FunctionPassManager) is created
to hold the passes used for optimizing a function. The FunctionPassManager supports running the passes
to transform a function into the optimized form. Since a pass manager is tied to the module and, for JIT
support, each function is generated into its own module a new method in the code generator is used to create
the module and initialize the pass manager.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#InitializeModuleAndPassManager)]

Creating the pass manager isn't enough to get the optimizations. Something needs to actually provide the
pass manager with the function to optimize. The most sensible place to put that is as the last step of
generating the function.

```C#
FunctionPassManager.Run( function );
```

This will run the passes defined when the FunctionPassManager was created, resulting in better generated code.

```llvm
define double @test(double %x) {
entry:
        %addtmp = fadd double %x, 3.000000e+00
        %multmp = fmul double %addtmp, %addtmp
        ret double %multmp
}
```

The passes eliminate the redundant add instructions to produce a simpler, yet still correct representation
of the generated code. LLVM provides a wide variety of optimization passes. Unfortunately not all are well
documented, yet. Looking into what Clang uses is helpful as is using the LLVM 'opt.exe' tool to run passes
individually or in various combinations and ordering to see how well it optimizes the code based on what
your front-end generates. (This can lead to changing the passes and ordering, as well as changes in what
the front-end generates so that the optimizer can handle the input better) This is not an exact science
with a one size fits all kind of solution. There are many common passes that are likely relevant to all
languages. Though the ordering of them may differ depending on the needs of the language and runtime.
Getting, the optimizations and ordering for a given language is arguably where the most work lies in
creating a production quality language using LLVM.

## Adding JIT Compilation
Now that the code generation produces optimized code, it is time to get to the fun part - executing code!
The basic idea is to allow the user to type in the Kaleidoscope code as supported thus far and it will
execute to produce a result. Unlike the previous chapters, instead of just printing out the LLVM IR
representation of a top level expression it is executed and the results are provided back to the user.

### Main Driver
The changes needed to the main driver are pretty simple, mostly consisting of removing a couple lines of
code that print out the LLVM IR for the module at the end and for each function when defined. The code
already supported showing the results if it was a floating point value by checking if the generated value
is a [ConstantFP](xref:Llvm.NET.Values.ConstantFP). We'll see a bit later on why that is a ConstantFP
value.

### Code Generator
The code generation needs an update to support using a JIT engine to generate and execute the Kaleidescope
code provided by the user.

To use the Optimization transforms the generator needs a new namespace using declaration.

```C#
using Llvm.NET.Transforms;
```

#### Generator fields
To begin with, the generator needs some additional members, including the JIT engine.

[!code-csharp[PrivateMembers](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#PrivateMembers)]

The JIT engine is retained for the generator to use. The same engine is retained for the lifetime of the
generator so that functions are added to the same engine and can call functions previously added. The JIT
provides a 'handle' for every module added, which is used to reference the module in the JIT, this is
normally used to remove the module from the JIT engine when re-defining a function. Thus, a map of the
function names and the JIT handle created for them is maintained. Additionally, a collection of defined
function prototypes is retained to enable matching a function call to a previously defined function.
Since the JIT support uses a module per function approach, lookups on the current module aren't sufficient.

#### Generator initialization
The initialization of the generator requires updating to support the new members.

[!code-csharp[Initialization](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#Initialization)]
The bool indicating if optimizations are enabled or not is stored and an initial module and pass manager
is created.

The option to disable optimizations is useful for debugging the code generation itself as optimizations
can alter or even eliminate incorrectly generated code. Thus, when modifying the generation itself, it
is useful to disable the optimizations.

#### JIT Engine
The JIT engine itself is a class provided in the Kaleidoscope.Runtime library derived from the Llvm.NET
OrcJIT engine.

[!code-csharp[Kaleidoscope JIT](../../../Samples/Kaleidoscope/Kaleidoscope.Runtime/KaleidoscopeJIT.cs)]

[OrcJit](xref:Llvm.NET.JIT.OrcJit) provides support for declaring functions that are external to the JIT
that the JIT'd module code can call. For Kaleidoscope, two such functions are defined directly in
KaleidoscopeJIT (putchard and printd), which is consistent with the same functions used in the official
LLVM C++ tutorial. Thus, allowing sharing of samples between the two. These functions are used to provide
rudimentary console output support.

> [!WARNING]
> All such methods implemented in .NET must block any exception from bubbling out of the call as the JIT
> engine doesn't know anything about them and neither does the Kaleidoscope language. Exceptions thrown
> in these functions would produce undefined results, at best - crashing the application.

#### PassManager
Every time a new function definition is processed the generator creates a new module and initializes
the function pass manager for the module. This is done is a new method InitializeModuleAndPassManager()

[!code-csharp[Initialization](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#InitializeModuleAndPassManager)]

The module creation is pretty straight forward, of importance is the layout information pulled from the
target machine for the JIT and applied to the module. 

Once the module is created, the [FunctionPassManager](xref:Llvm.NET.Transforms.FunctionPassManager) is
constructed. If optimizations are not disabled, the optimization passes are added to the pass manager.
The set of passes used is a very basic set since the Kaleidoscope language isn't particularly complex
at this point.


#### Generator Dispose
Since the JIT engine is disposable, the code generators Dispose() method must now call the
Dispose() method on the JIT engine.

[!code-csharp[Dispose](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#Dispose)]

#### Generate Method
To actually execute the code the generated modules are added to the JIT. If the function is an 
anonymous top level expression, it is eagerly compiled and a delegate is retrieved from the JIT
to allow calling the compiled function directly. The delegate is then called to get the result. Once an anonymous function produces
a value, it is no longer used so is removed from the JIT and the result value returned. For other functions
the module is added to the JIT and the function is returned.

For named function definitions, the module is lazy added to the JIT as it isn't known if/when the functions
is called. The JIT engine will compile modules lazy added into native code on first use. (Though if the
function is never used, then creating the IR module was wasted. ([Chapter 7.1](Kaleidoscope-ch7.1.md) has a
solution for even that extra overhead - truly lazy JIT). Since Kaleidoscope is generally a dynamic language
it is possible and reasonable for the user to re-define a function (to fix an error, or provide a completely
different implementation all together). Therefore, any named functions are removed from the JIT, if they
existed, before adding in the new definition. Otherwise the JIT resolver would still resolve to the previously
compiled instance.

[!code-csharp[Generate](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#Generate)]

Keeping all the JIT interaction in the generate method isolates the rest of the generation from any
awareness of the JIT. This will help when adding truly lazy JIT compilation in [Chapter 7.1](Kaleidoscope-ch7.1.md)
and AOT compilation in [Chapter 8](Kaleidoscope-ch8.md)

#### Function call expressions
Since functions are no longer collected into a single module the code to find the target for a function
call requires updating to lookup the function from a collection of functions mapped by name.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#FunctionCallExpression)]

This will lookup the function prototype by name and call the GetOrDeclareFunction() with the prototype
found. If the prototype wasn't found then it falls back to the previous lookup in the current module.
This fall back is needed to support recursive functions where the referenced function actually is in the
current module.

#### GetOrDeclareFunction()
Next is to update the GetOrDeclareFunction() to handle mapping the functions prototype and re-definition
of functions.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#GetOrDeclareFunction)]

This distinguishes the special case of an anonymous top level expression as those are never added to the
prototype maps. They are only in the JIT engine long enough to execute once and are then removed. Since
they are, by definition, anonymous they can never be referenced by anything else.

#### Function Definitions
Visiting a function definition needs to add a call to the function pass manager to run the optimization
passes for the function. This, makes sense to do, immediately after completing the generation of the function.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#FunctionDefinition)]


## Conclusion
While the amount of words needed to describe the changes to support optimization and JIT execution here
isn't exactly small, the actual code changes required really are. The Parser and JIT engine do all the
heavy lifting. Llvm.NET provides a clean interface to the JIT that fits with common patterns and runtime
support for .NET. Very cool, indeed!
