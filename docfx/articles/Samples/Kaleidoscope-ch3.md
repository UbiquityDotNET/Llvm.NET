# Chapter 3 - Code Generation to LLVM IR
This chapter focuses on the basics of transforming the ANTLR parse tree into LLVM IR. The general goal is to
parse Kaleidoscope source code to generate a [BitcodeModule](xref:Llvm.NET.BitcodeModule) representing the
source as LLVM IR.

## Basic code flow
The entry point for the application is the standard Main() function. This implementation is generally the same
for most of the remaining chapters. Chapter specific topics will cover any particular variations relevant to
the changes for that chapter, if any are needed.

The Main function starts out by calling WaitForDebugger(). This is a useful utility that doesn't do anything in a
release build, but in debug builds will check for an attached debugger and if none is found it will wait for one.
This works around a missing feature of the .NET Standard C# project system that does not support launching mixed
native+managed debugging. When you need to go all the way into debugging the LLVM code, you can launch the debug
version of the app without debugging, then attach to it and select native and managed debugging. (Hopefully this
feature will be restored to these projects in the future so this trick isn't needed...)

### Initializing Llvm.NET
The underlying LLVM library requires initialization for it's internal data, furthermore Llvm.NET must load
the actual underlying DLL specific to the current system architecture. Thus, the library as a whole requires
initialization.

```C#
using static Llvm.NET.StaticState;
// [...]

using( InitializeLLVM() )
{
    // [...]
}
```

The initialization returns an IDisposable so that the calling application can shutdown/cleanup resources
and potentially re-initialize for a different target if desired. This application only needs to generate one
module and exit so it just applies a standard C# `using` scope to ensure proper cleanup.

### Initializing Targets
LLVM supports a number of target architectures, however for the Kaleidoscope generation the only supported
target is the one the host application is running on. So, only the native target is registered.

``` C#
    RegisterNative();
```

### Generator and REPL loop
This chapter supports the simple expressions of the language that are parsed and generated to an LLVM
[Value](xref:Llvm.NET.Values.Value). This forms the foundation of the Kaleidoscope samples outer generation
loop. Subsequent, chapters will focus on additional functionality including JIT compilation, Debugging information,
Native Module generation.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/Program.cs#generatorloop)]

### Processing output for results
As the REPL loop recognizes the input and generates output it notifies the application of the output so that
the application can use the results. (Typically by showing the results to the user in some fashion). In Chapter
2 this was used to generate representations of the raw parse tree to aid in comprehending the language. For this
chapter it is used to print information about what was generated from the input to the parser.

[!code-csharp[ResultProcessing](../../../Samples/Kaleidoscope/Chapter3/Program.cs#ResultProcessing)]

## Code generation

### Initialization
The code generation maintains state for the transformation as private members.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#PrivateMembers)]

These are initialized in the constructor

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#Initialization)]

The exact set of members varies for each chapter but the basic ideas remain across each chapter.

|Name | Description |
|-----|-------------|
| RuntimeState | Contains information about the language and dynamic runtime state needed for resolving operator precedence |
| Context | Current [Context](xref:Llvm.NET.Context) for LLVM generation |
| Module | Current [BitcodeModule](xref:Llvm.NET.BitcodeModule) to generate LLVM IR in|
| InstructionBuilder | Current  [InstructionBuilder](xref:Llvm.NET.Instructions.InstructionBuilder) used to generate LLVM IR instructions |
| NamedValues | Contains a mapping of named variables to the generated [Value](xref:Llvm.NET.Values.Value) |

### Generate Method
The Generate method is used by the REPL loop to generate the final output from a parse tree. The common implementation simply checks for
syntax errors in the tree, and if there were any returns null otherwise performs a visitor walk against the parse tree to generate a result.
The syntax errors check is done in the generator to all generators to handle invalid input or partial input gracefully or in some special fashion
determined by the generator implementation.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#Generate)]

### Constant expression
In Kaleidoscope all values are floating point and constants are represented in LLVM IR as [ConstantFP](xref:Llvm.NET.Values.ConstantFP)
The parse tree node for a constant is extended to provide the value of the constant as a C# `double`.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/KaleidoscopeParser.ConstExpressionContext.cs)]

Generation of the LLVM IR for a constant is quite simple.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#VisitConstExpression)]

Note that the constant value is uniqued in LLVM so that multiple calls given the same input value will produce the same LLVM Value.
LLvm.NET honors this and is implemented in a way to ensure that reference equality reflects the identity of the uniqued values correctly.

### Variable expression
References to variables in Kaleidoscope, like most other languages, use a name. In this chapter the support of variables is rather simple.
The Variable expression generator assumes the variable is declared somewhere else already and simply looks up the value from the private map.
At this stage of the development of Kaleidoscope the only place where the named values are generated are function arguments, later chapters will
introduce loop induction variables and variable assignment. The implementation uses standard TryGet pattern to get the value or throw an exception
if the variable doesn't exist.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#VisitVariableExpression)]

### Expression
Things start to get a good bit more interesting with binary operators. The parse tree node for an expression contains
support for walking the chain of operators that form an expression in left to right order, accounting for precedence.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Kaleidoscope.Parser/KaleidoscopeParser.ExpressionContext.cs)]

Generation of an expression consists of a pair of related methods.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#VisitExpression)]

VisitExpression() will create a value for the left most 'Atom' and then use that with the operator and right hand side value
for the next expression. The left hand side is updated based on the result of each operation so that once all operations in the
expression are evaluated the `lhs` variable retains the result value for the entire expression. Each operator result is generated
through the private utility method EmitBinaryOperator().

EmitBinaryOperator is responsible for generating the LLVM IR representation of a binary operator.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#EmitBinaryOperator)]

The process of transforming the operator starts by generating an LLVM IR Value from the right-hand side parse tree.
A simple switch statement based on the token type of the operator is used to generate the actual LLVM IR instruction(s)
for the operator. 

LLVM has strict rules on the operators and their values for the IR, in particular the types of the operands must be identical
and, usually must also match the type of the result. For the Kaleidoscope language that's easy to manage as it only supports one
data type. Other languages might need to insert additional conversion logic as part of emitting the operators. 

The Generation of the IR instructions uses the current InstructionBuilder and the [RegisterName](xref:Llvm.NET.Values.ValueExtensions.RegisterName``1(``0,System.String))
extension method to provide a name for the result in LLVM IR. The name helps with readability of the IR when generated in the
textual form of LLVM IR assembly. A nice feature of LLVM is that it will automatically handle duplicate names by appending a value to
the name automatically so that generators don't need to keep track of the names to ensure uniqueness. 

The `Less` operator uses a floating point Unordered less than IR instruction followed by an integer to float cast to translate the
LLVM IR i1 result into a floating point value needed by Kaleidoscope.

The `^` operator for exponentiation uses the `llvm.pow.f64` intrinsic to perform the exponentiation as efficiently as the back-end
generator can.


### Function Declarations
Function declaration is performed on prototypes for the function. This includes External function declarations as well as the
prototype portion of a function definition.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#FunctionDeclarations)]

An external declarations has just the prototype so the visitor for those simply extracts the signature prototype to visit and returns.
The function prototype visitor uses a private utility GetOrDeclareFunction to get an existing function or declare a new one.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#GetOrDeclareFunction)]

GetOrDeclareFunction() will first attempt to get an existing function and if found returns that function. Otherwise it creates
a function signature type then adds a function to the module with the given name and signature and adds the parameter names to
the function. In LLVM the signature only contains type information and no names, allowing for sharing the same signature for
completely different functions.

### Function Definition
Functions with bodies (e.g. not just a declaration to a function defined elsewhere) are handled via the VisitFunctionDefinition()
Method.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#VisitFunctionDefinition)]

VisitFunctionDefinition() simply extracts the function prototype from the parse tree node and processes that to produce a Function
declaration. The declaration and the expression representing the body of the function is then passed to the private DefineFunction
that does the real work of defining the function.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#DefineFunction)]

DefineFunctio() first tests to see that the function is a declaration (e.g. does not have a body) as Kaleidoscope doesn't support
any sort of overloaded functions.

The generation of a function starts by constructing a basic block for the entry point of the function and attaches the InstructionBuilder
to the end of that block. (It's empty so it is technically at the beginning but placing it at the end it will track the end position as new
instructions are added so that each instruction added will go on the end of the block). At this point there will only be the one entry block
as the language doesn't have support for control flow. (That is introduced in [Chapter 5](Kaleidoscope-ch5.md))

The NamedValues map is cleared and each of the parameters is mapped in the NamedValues map to its argument value in IR. The body of the function
is visited to produce an LLVM Value. The visiting will, in turn add instructions, and possibly new blocks, as needed to represent the expression
in proper execution order.

If generating the body results in an error, then the function is removed from the parent and null is returned. This allows the user to define the
function again.

Finally, a return instruction is applied to return the result of the expression followed by a verification of the
function to ensure internal consistency. (Generally the verify is not used in production releases as it is an expensive operation to perform on
every function. But when building up a language generator it is quite useful to detect errors early.)

### Top Level Expression
Top level expressions in Kaleidoscope are transformed into an anonymous function definition by the VisitTopLevelExpression visitor method.

[!code-csharp[Main](../../../Samples/Kaleidoscope/Chapter3/CodeGenerator.cs#VisitTopLevelExpression)]

This simply creates a new prototype with a unique name then uses that to declare and define a function with the expression as the body. This
re-use of the GetOrDeclareFunction() and DefineFunction() helps keep the generation clean and makes future additions simpler.


## Examples

```Console
Llvm.NET Kaleidoscope Interpreter - SimpleExpressions
Ready>4+5;
Defined function: anon_expr_0

define double @anon_expr_0() {
entry:
  ret double 9.000000e+00
}

Ready>def foo(a b) a*a + 2*a*b + b*b;
Defined function: foo

define double @foo(double %a, double %b) {
entry:
  %multmp = fmul double %a, %a
  %multmp1 = fmul double 2.000000e+00, %a
  %multmp2 = fmul double %multmp1, %b
  %addtmp = fadd double %multmp, %multmp2
  %multmp3 = fmul double %b, %b
  %addtmp4 = fadd double %addtmp, %multmp3
  ret double %addtmp4
}

Ready>def bar(a) foo(a, 4.0) + bar(31337);
Defined function: bar

define double @bar(double %a) {
entry:
  %calltmp = call double @foo(double %a, double 4.000000e+00)
  %calltmp1 = call double @bar(double 3.133700e+04)
  %addtmp = fadd double %calltmp, %calltmp1
  ret double %addtmp
}

Ready>extern cos(x);
Defined function: cos

declare double @cos(double)

Ready>cos(1.234);
Defined function: anon_expr_1

define double @anon_expr_1() {
entry:
  %calltmp = call double @cos(double 1.234000e+00)
  ret double %calltmp
}

Ready>^Z
```
```llvm
; ModuleID = 'Kaleidoscope'
source_filename = "Kaleidoscope"

define double @anon_expr_0() {
entry:
  ret double 9.000000e+00
}

define double @foo(double %a, double %b) {
entry:
  %multmp = fmul double %a, %a
  %multmp1 = fmul double 2.000000e+00, %a
  %multmp2 = fmul double %multmp1, %b
  %addtmp = fadd double %multmp, %multmp2
  %multmp3 = fmul double %b, %b
  %addtmp4 = fadd double %addtmp, %multmp3
  ret double %addtmp4
}

define double @bar(double %a) {
entry:
  %calltmp = call double @foo(double %a, double 4.000000e+00)
  %calltmp1 = call double @bar(double 3.133700e+04)
  %addtmp = fadd double %calltmp, %calltmp1
  ret double %addtmp
}

declare double @cos(double)

define double @anon_expr_1() {
entry:
  %calltmp = call double @cos(double 1.234000e+00)
  ret double %calltmp
}

```
