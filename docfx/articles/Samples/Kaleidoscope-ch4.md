# 4. Kaleidoscope: Adding JIT and Optimizer Support
This chapter of the Kaleidoscope tutorial introduces JIT compilation and optimizations of the generated code. As, such this is the first variant
of the language implementation where you can actually execute the Kaleidoscope code. Thus, this is a bit more fun than the others as you can finally
get to see the language working for real.

## Constant Folding
If you studied the LLVM IR generated from the previous chapters you will see that it isn't particularly well optimized. Though, there is one case
where it does do some nice optimization for us. For example:
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
That's not exactly what the parse tree would suggest. The [InstructionBuilder](xref:Llvm.NET.Instructions.InstructionBuilder) automatically performs
an optimization technique know as 'Constant Folding'. This optimization is very important, in fact many compilers implement the folding directly into
the generation of the Abstract Syntax Tree (AST). With LLVM that isn't necessary as it is automatically provided for you (no extra charge!).

Obviously constant folding isn't the only possible optimization and InstructionBuilder only operates on the instructions as they are built. So,
there are limits on what InstructionBUilder can do. For example:

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
In this case the operand of the additions are identical. Ideally this is generated as `temp = x+3; result = temp*temp;` rather than computing X+3 twice.
This isn't something that InstructionBuilder can do. Ultimately this requires two transformations:
  1. Re-association of expressions to make the additions lexically identical (e.g. recognize that x+3 == 3+x )
  1. Common Subexpression Elimination to remove the redundant add instruction.

Fortunately, LLVM provides a very broad set of optimization transformations that can handle this and many other scenarios.

## LLVM Optimization Passes
LLVM provides many different optimization passes, each handling a specific scenario with different trade-offs. One of the values of LLVM as a
general compilation back-end is that it doesn't enforce any particular set of optimizations. By default there aren't any optimizations (Other
than the obvious constant folding built into the InstructionBuilder). All optimizations are entirely in the hands of the front-end application.
The compiler implementor controls what passes are applied, and in what order they are run. This ensures that the optimizations are tailored
to correctly meet the needs of the language and runtime environment.

For Kaleidoscope, optimizations are limited to a single function as they are generated when the user types them in on the command line. Ultimate,
whole program optimization is off the table (You never know when the user will enter the last expression so it is incorrect to eliminate
unused functions). In order to support per-function optimization a [FunctionPassManager](xref:Llvm.NET.Transforms.FunctionPassManager) is created to hold
the passes used for optimizing a function. The FunctionPassManager supports running the passes added to it against a function to transform it into
the optimized form. Since a pass manager is tied to the module and, for JIT support each function is generated into its own module a new function
is added to create the module and initialize the pass manager.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#InitializeModuleAndPassManager)]

Creating the pass manager isn't enough to get the optimizations. Something needs to actually provide the pass manager with the function to optimize.
The most sensible place to put that is as the last step of generating the function.

```C#
FunctionPassManager.Run( function );
```

This will run the passes defined in when the FunctionPassManager was created, resulting in better generated code.

```llvm
define double @test(double %x) {
entry:
        %addtmp = fadd double %x, 3.000000e+00
        %multmp = fmul double %addtmp, %addtmp
        ret double %multmp
}
```

The passes eliminate the redundant add to produce a simpler, yet still correct representation of the generated code.
LLVM provides a wide variety of optimization passes. Unfortunately not all are well documented, yet. Looking into what Clang uses
is helpful as is using the LLVM 'opt.exe' tool to run passes individually or in various combinations and ordering to see how
well it optimizes the code based on what your front-end generates. (This can lead to changing the passes and ordering, as well as
changes in what the front-end generates so that the optimizer can handle the input better) This is not an exact science with a one
size fits all kind of solution. There are many common passes that are likely relevant to all languages. Though the ordering of them
may differ depending on the needs of the language.

## Adding JIT Compilation
Now that the code generation produces optimized code, it is time to get the fun part - executing code!
The basic idea is to allow the user to type in the Kaleidoscope code as supported thus far. However, instead of just printing out the
LLVM IR representation of a top level expression it is executed and the results are provided back to the user.

### Main Driver
The changes needed to the main driver are pretty simple, mostly consisting of removing a couples lines of code that print out the
LLVM IR for the module at the end and for each function when defined. The code already supported showing the results if it was
a floating point value by checking if the generated value is a [ConstantFP](xref:Llvm.NET.Values.ConstantFP). We'll see a bit later
on why that is a ConstantFP value.

### Code Generator
The code generation needs an update to support using a JIT engine to generate and execute the Kaleidescope code provided by the user.

To begin with the generator needs some additional members, including the JIT engine. 

```C#
private readonly KaleidoscopeJIT JIT;
private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap;
private FunctionPassManager FunctionPassManager;
private readonly PrototypeCollection FunctionPrototypes;

/// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
/// <returns>Result of evaluating the expression</returns>
[UnmanagedFunctionPointer( System.Runtime.InteropServices.CallingConvention.Cdecl )]
private delegate double AnonExpressionFunc( );
```

The JIT engine is retained for the generator to use. The same engine is retained for the lifetime of the generator so that functions are
added to the same engine and can call function previously added. The JIT provides a 'handle' for every module added, which is used to reference
the module in the JIT, this is normally used to remove the module from the JIT engine when re-defining a function. Thus, a map of the function
names and the JIT handle created for them is maintained. Additionally, a collection of defined function prototypes is retained to enable matching
a function call to a previously defined function. Since the JIT support uses a module per function approach, lookups on the current module aren't
sufficient. Finally, a native function call delegate is defined for top level anonymous expressions. So that, after the JIT engine has generated
the code, the application can call it through a delegate.

The initialization of the generator requires updating to support the additional members.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#Initialization)]

Additionally, since the JIT engine is disposable, the code generators Dispose() method must call the Dispose() method on the JIT engine.

The JIT engine itself is a class provided in the Kaleidoscope.Runtime library to wrap the LLVM ORC JIT engine

[!code-csharp[Main](../../../Samples/Kaleidoscope/Kaleidoscope.Runtime/KaleidoscopeJIT.cs)]

[OrcJit](xref:Llvm.NET.JIT.OrcJit) provides support for declaring functions that are external to the JIT that the JIT'd module code can call.
For Kaleidoscope, two such functions are defined directly in KaleidoscoptJIT (putchard and printd), which is consistent with the same functions
used in the official LLVM C++ tutorial. Thus allowing sharing of samples between the two.

>[!WARNING]
>All such methods implented in .NET must block any exception from bubling out of the call as the JIT engine doesn't know anything about them
>and neither does the Kaleidoscope language. Exceptions thrown in these functions would produce undefined results, at best crashing the application.

Since functions are no longer collected into a single module the code to find the target for a function call requires updating to lookup the function.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#FindCallTarget)]

This will lookup the function prototype by name and call the GetOrDeclareFunction() with the prototype found. If the prototype wasn't found then
it falls back to the previous lookup in the current module. This fall back is needed to support recursive functions where the function actually
is in the current module.

Next is to update the GetOrDeclareFunction() to handle mapping the functions prototype and re-definition of functions.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#GetOrDeclareFunction)]

This distinguishes the special case of an anonymous top level expression as those are never added to the prototype maps. They are only in the JIT
engine long enough to execute once and are then removed.

The DefineFunction needs updating to support adding/removing the function's module in the JIT, tracking the JIT handle for the module and ultimately
re-initializing a new module and pass manager for the next function.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#DefineFunction)]

>[!NOTE]
>Instead of just returning the function DefineFunction() now returns a Tuple with the Function and the [IJitModuleHandle](xref:Llvm.NET.JIT.IJitModuleHandle)
>the JIT generated for the module. The handle is used in DefineFunction() later to remove the module whenever re-dfining an existing function.

That covers the modifications to the core generation utility functions for JIT. Only two of the Visitor methods require additional updates to complete the JIT
engine support.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#VisitFunctionDefinition)]

VisitFunctionDefinition() requires only a small update to extract the function from the tuple returned by DefineFunction(), the handle isn't needed here
as it is mapped already in DefineFunction().

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter4/CodeGenerator.cs#VisitTopLevelExpression)]

Visiting the top level expression needs some additional work to make the JIT actually work. To will generate the function, as before, however
it will capture the JIT handle so it can remove it. The real 'magic' of the JIT happens in the next 2 lines:

```C#
var nativeFunc = JIT.GetDelegateForFunction<AnonExpressionFunc>( proto.Identifier.Name );
var retVal = Context.CreateConstant( nativeFunc( ) );
```

This asks the JIT engine to provide a delegate matching the AnonExpressionFunc signature for a function with the name of the anonymous function just
created (and compiled to native code). The function is the called through the delegate to produce a result. Since the result is a raw native double
and all of the visitor methods must return a [Value](xref:Llvm.NET.Values.Value) a new [ConstantFP](xref:Llvm.NET.Values.ConstantFP) is created for
the result of the call.

After making the call to the generated function, the anonymous function is removed from the prototype mapping and then removed from the JIT module as
it is no longer needed.

## Conclusion
While the amount of words needed to describe the changes to support optimization and JIT execution here isn't exactly small, the actual code changes
required really are. The JIT engine does the heavy lifting and Llvm.NET provides a clean interface to the JIT that fits with common patterns and
runtime support for .NET. Very cool, indeed!
