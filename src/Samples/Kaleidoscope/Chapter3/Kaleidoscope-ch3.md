---
uid: Kaleidoscope-ch3
---

# 3. Kaleidoscope: Generating LLVM IR
This chapter focuses on the basics of transforming the ANTLR parse tree into LLVM IR. The general goal is
to parse Kaleidoscope source code to generate a [Module](xref:Ubiquity.NET.Llvm.Module) representing
the source as LLVM IR.

## Basic code flow
The Main function starts out by calling WaitForDebugger(). This is a useful utility that doesn't do
anything in a release build, but in debug builds will check for an attached debugger and, if none is found,
it will wait for one. This works around a missing feature of the .NET Standard C# project system that
does not support launching mixed native+managed debugging. When you need to go all the way into debugging
the LLVM code, you can launch the debug version of the app without debugging, then attach to it and
select native and managed debugging. (Hopefully this feature will be restored to these projects in the
future so this rather hacky trick isn't needed...)

>UPDATE:
>As of VS2019 this hack is no longer needed as it is now possible to set an SDK project to allow native
>debugging directly from the project's debugging settings page. (Yeah! :triumph:)

### Initializing Ubiquity.NET.Llvm
The underlying LLVM library requires initialization for it's internal data, furthermore Ubiquity.NET.Llvm must load
the actual underlying library specific to the current system architecture. Thus, the Ubiquity.NET.Llvm as a whole
requires initialization.

``` C#
using static Ubiquity.NET.Llvm.Interop.Library;

// [...]

using( InitializeLLVM() )
{
    // [...]
}
```

The initialization returns an IDisposable so that the calling application can shutdown/cleanup resources
and potentially re-initialize for a different target, if desired. This application only needs to generate
one module and exit so it just applies a standard C# `using` scope to ensure proper cleanup.

### Initializing Targets
LLVM supports a number of target architectures, however for the Kaleidoscope tutorials the only supported
target is the one the host application is running on. So, only the native target is registered.

``` C#
    RegisterNative();
```

### Generator and REPL loop
This chapter supports the simple expressions of the language that are parsed and generated to an LLVM
[Value](xref:Ubiquity.NET.Llvm.Values.Value). This forms the foundation of the Kaleidoscope samples outer
generation loop. Subsequent, chapters will focus on additional functionality including JIT compilation,
Debugging information, and Native Module generation. Processing the results for this chapter, is pretty simple, it just prints out a textual form of the 
generated LLVM IR.

[!code-csharp[Main](ReplEngine.cs)]

## Code generation

### Initialization
The code generation maintains state for the transformation as private members.

[!code-csharp[Main](CodeGenerator.cs#PrivateMembers)]

These are initialized in the constructor

[!code-csharp[Main](CodeGenerator.cs#Initialization)]

The exact set of members varies for each chapter but the basic ideas remain across each chapter.

|Name | Description |
|-----|-------------|
| RuntimeState | Contains information about the language and dynamic runtime state needed for resolving operator precedence |
| Context | Current [Context](xref:Ubiquity.NET.Llvm.Context) for LLVM generation |
| Module | Current [Module](xref:Ubiquity.NET.Llvm.Module) to generate LLVM IR in|
| InstructionBuilder | Current  [InstructionBuilder](xref:Ubiquity.NET.Llvm.Instructions.InstructionBuilder) used to generate LLVM IR instructions |
| NamedValues | Contains a mapping of named variables to the generated [Value](xref:Ubiquity.NET.Llvm.Values.Value) |

### Generate Method
The Generate method is used by the REPL loop to generate the final output from a parse tree. The common
implementation simply passes the tree to the AST generating parse tree visitor to generate the AST and
process the AST nodes from that. Due to the simplicity of the Kaleidoscope language the AST is more of
a List than a tree. In fact, the AstBuilder creates an enumerable sequence of nodes that are either a
function declaration or a function definition. For the interactive mode only a single element is parsed
at a time. However, when doing Ahead of Time (AOT) compilation in [Chapter 8](xref:Kaleidoscope-ch8)
this sequence can contain many declarations and definitions in any order. To handle the different node
types the generate method simply uses pattern matching to detect the type of node to dispatch to a visitor
function for that kind of node.

[!code-csharp[Main](CodeGenerator.cs#Generate)]

### Function Declarations
Function declarations don't actually generate any code. Instead they are captured and added to a collection
of declarations used in validating subsequent function calls when generating the AST for function definitions.

[!code-csharp[Main](../Kaleidoscope.Grammar/AST/Prototype.cs)]

### Function Definition
Functions with bodies (e.g. not just a declaration to a function defined elsewhere) are handled via the
VisitFunctionDefinition() Method.

[!code-csharp[Main](CodeGenerator.cs#FunctionDefinition)]

VisitFunctionDefinition() simply extracts the function prototype from the AST node. A private utility 
method GetOrDeclareFunction() is used to get an existing function or declare a new one.

[!code-csharp[Main](CodeGenerator.cs#GetOrDeclareFunction)]

GetOrDeclareFunction() will first attempt to get an existing function and if found returns that function.
Otherwise it creates a function signature type then adds a function to the module with the given name and
signature and adds the parameter names to the function. In LLVM the signature only contains type information
and no names, allowing for sharing the same signature for completely different functions.

The function and the expression representing the body of the function is then used to emit IR for the function.

The generation verifies that the function is a declaration (e.g. does not have a body) as Kaleidoscope doesn't
support any sort of overloaded functions.

The generation of a function starts by constructing a basic block for the entry point of the function and
attaches the InstructionBuilder to the end of that block. (It's empty so it is technically at the beginning
but placing it at the end it will track the end position as new instructions are added so that each instruction
added will go on the end of the block). At this point there will only be the one block as the language
doesn't yet have support for control flow. (That is introduced in [Chapter 5](xref:Kaleidoscope-ch5))

The NamedValues map is cleared and each of the parameters is mapped in the NamedValues map to its argument
value in IR. The body of the function is visited to produce an LLVM Value. The visiting will, in turn add
instructions, and possibly new blocks, as needed to represent the body expression in proper execution order.

If generating the body results in an error, then the function is removed from the parent and the exception
propagates up. This allows the user to define the function again, if appropriate.

Finally, a return instruction is applied to return the result of the expression followed by a verification
of the function to ensure internal consistency. (Generally the verify is not used in production releases
as it is an expensive operation to perform on every function. But when building up a language generator
it is quite useful to detect errors early.)

#### Top Level Expression
Top level expressions in Kaleidoscope are transformed into an anonymous function definition by the
AstBuilder. Since this chapter is focused on generating the IR module there isn't any special handling
needed for a top level expression - they are simply just another function definition. (JIT execution of
the top level expression comes in the next chapter)

### Constant expression
In Kaleidoscope all values are floating point and constants are represented in LLVM IR as [ConstantFP](xref:Ubiquity.NET.Llvm.Values.ConstantFP)
The AST provides the value of the constant as a C# `double`.

[!code-csharp[Main](../Kaleidoscope.Grammar/AST/ConstantExpression.cs)]

Generation of the LLVM IR for a constant is quite simple.

[!code-csharp[Main](CodeGenerator.cs#ConstantExpression)]

> [!NOTE]
> The constant value is uniqued in LLVM so that multiple calls given the same input value will
> produce the same LLVM Value. Ubiquity.NET.Llvm honors this and is implemented in a way to ensure that reference
> equality reflects the identity of the uniqued values correctly.

### Variable reference expression
References to variables in Kaleidoscope, like most other languages, use a name. In this chapter the support
of variables is rather simple. The Variable expression generator assumes the variable is declared somewhere
else already and simply looks up the value from the private map. At this stage of the development of
Kaleidoscope the only place where the named values are generated are function arguments, later chapters
will introduce loop induction variables and variable assignment. The implementation uses a standard TryGet
pattern to retrieve the value or throw an exception if the variable doesn't exist.

[!code-csharp[Main](CodeGenerator.cs#VariableReferenceExpression)]

### Binary Operator Expression
Things start to get a good bit more interesting with binary operators. The AST node for an expression
is a simple empty "tagging" interface. Since the interface also requires the IAstNode interface it contains
support for walking the chain of operators that form an expression in left to right order, accounting
for precedence.

[!code-csharp[Main](../Kaleidoscope.Grammar/AST/IExpression.cs)]

Generation of an expression consists a simple visitor method to emit the code for the operands and
the actual operator.

[!code-csharp[Main](CodeGenerator.cs#BinaryOperatorExpression)]

The process of transforming the operator starts by generating an LLVM IR Value from the right-hand side
expression. A simple switch statement based on the token type of the operator is used to generate the
actual LLVM IR instruction(s) for the operator.

LLVM has strict rules on the operators and their values for the IR, in particular the types of the
operands must be identical and, usually must also match the type of the result. For the Kaleidoscope
language that's easy to manage as it only supports one data type. Other languages might need to insert
additional conversion logic as part of emitting the operators. (Kaleidoscope does this for boolean
values when supporting conditional control flow in [Chapter 5](xref:Kaleidoscope-ch5))

The Generation of the IR instructions uses the current InstructionBuilder and the [RegisterName](xref:Ubiquity.NET.Llvm.Values.ValueExtensions.RegisterName``1(``0,System.String))
extension method to provide a name for the result in LLVM IR. The name helps with readability of the IR
when generated in the textual form of LLVM IR assembly. A nice feature of LLVM is that it will automatically
handle duplicate names by appending a value to the name automatically so that generators don't need to
keep track of the names to ensure uniqueness.

The `Less` operator uses a floating point `unordered less than` IR instruction followed by an integer to
float cast to translate the LLVM IR i1 result into a floating point value needed by Kaleidoscope.

The `^` operator for exponentiation uses the `llvm.pow.f64` intrinsic to perform the exponentiation as
efficiently as the back-end generator can.

## Examples

```Console
Ubiquity.NET.Llvm Kaleidoscope Interpreter - SimpleExpressions
Ready># simple top level expression
>4+5;
Defined function: __anon_expr$0

define double @"__anon_expr$0"() {
entry:
  ret double 9.000000e+00
}

Ready>
Ready># function definitions
>def foo(a b) a*a + 2*a*b + b*b;
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

Ready>
Ready>def bar(a) foo(a, 4.0) + bar(31337);
Defined function: bar

define double @bar(double %a) {
entry:
  %calltmp = call double @foo(double %a, double 4.000000e+00)
  %calltmp1 = call double @bar(double 3.133700e+04)
  %addtmp = fadd double %calltmp, %calltmp1
  ret double %addtmp
}

Ready>
Ready># external declaration
>extern cos(x);
Defined function: cos

declare double @cos(double)

Ready>
Ready># calling external function
>cos(1.234);
Defined function: __anon_expr$1

define double @"__anon_expr$1"() {
entry:
  %calltmp = call double @cos(double 1.234000e+00)
  ret double %calltmp
}

Ready>
```
