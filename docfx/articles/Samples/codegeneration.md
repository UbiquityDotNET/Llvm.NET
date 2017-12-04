# CodeGenWithDebugInfo
Sample application to generate target machine code. The sample is
provided in the [source tree](https://github.com/UbiquityDotNET/Llvm.NET/tree/master/Samples/CodeGenWithDebugInfo).

This sample generates LLVM IR equivalent to what Clang will generate for a sample C file. While it doesn't parse
the C File it does show all the steps and techniques for using Llvm.NET to generate the LLVM IR with debug
information adn ultimately the target machine code.

## Example C Code
The CodeGenWithDebugInfo sample will generate LLVM IR and machine code for the following sample "C" code. (This
code file is provided in the source tree along with a script file to compile it for comparing output with Clang)

[!code-c[Main](../../../Samples/CodeGenWithDebugInfo/Support Files/test.c)]

This sample supports targetting two different processor types x64 and ARM Cortex-M3

## Initializing Llvm.NET
The underlying LLVM library requires initialization for it's internal data, furthermore Llvm.NET must load
the actual underlying DLL specific to the current system architecture. Thus, the library as a whole requires
initialization.

```C#
using static Llvm.NET.StaticState;

using( InitializeLLVM() )
{
    // [...]
}
```

The initialization returns an IDisposable so that the calling application can shutdown/cleanup resources
and potentially re-init for a different target if desired. This application only needs to generate one
module and exit so it just applies a standard C# `using` scope to ensure proper cleanup.

### Target specific details
In order to isolate the specific details of the target architecture the application uses an interface that
contains properties and methods to handle target specific support. Furthermore, an application may not need
to use all of the possible target architectures so the application selects to register/initialize support for
specific targets. This reduces startup time and resource commitments to only what is required by the application.
In this sample that is handled in the contructor of the target dependent details. Most compiler type applications
would allow command line options for the CPU target variants and feature sets. For this sample those are just
hard coded into the target details class to keep things simple and focused on the rest of the code generation.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/ITargetDependentDetails.cs#ITargetDependentDetails)]

This interface isolates the rest of the code from knowing which architecture is used, and theoretically
could include support for additional targets beyond the two in the sample source.

The sample determines which target to use based on the second command line argument to the application

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#TargetDetailsSelection)]

## Creating the BitcodeModule
To generate code in Llvm.NET a [BitcodeModule](xref:Llvm.NET.BitcodeModule) is required as
a container for the LLVM IR. To create a module a [Context](xref:Llvm.NET.Context) is
required.

>[!NOTE]
>The Context and BitcodeModule are Disposable types in Llvm.NET to manage some complex and
hidden ownership transfers that can hapen with the different froms of JIT/Execution engines.
This may not always be true in future versions of the library, but for now they must be disposable.

>[!CAUTION]
>A Context is, by design, **NOT** a thread safe type. It is designed to contain various interned
objects in LLVM. All modules are owned by exactly one Context. Applications can create any
number of threads and create a context for each one. However, threads must not reference the
context of another thread nor reference any of the objects created within another thread's
context. This is a fundamental design of LLVM and reduces the complexity of attempting to
manage collections of objects and interning them in a thread safe manner. Applicatins instead
just create a context per thread if needed.


To generate code for a particular target the application initializes the module to include the
source file name that it was generated from, the [Triple](xref:Llvm.NET.Triple) that describes
the target and a target specific [DataLayout](xref:Llvm.NET.DataLayout). The sample application
extracts these from the [TargetMachine](xref:Llvm.NET.TargetMachine) provided by the target
details interface for the selected target.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingModule)]

## Creating the DICompileUnit
LLVM Debug information is all scoped to a top level [DICompileUnit](xref:Llvm.NET.DebugInfo.DICompileUnit).
There is exactly one DICompileUnit for a BitcodeModule and all debug information metadata is ultimately
a child of that unit. The sample creates the compliation unit just after the module is created and the
target specific information is added to it. In this sample there is a direct 1:1 corelation between the
compile unit and the source file so it creates a [DIFile](xref:Llvm.NET.DebugInfo.DIFile) for the source
at the same time.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingCompileUnit)]

## Creating basic types with debug information
In LLVM types are fairly minimalistic and only contain the basic strctural information for generating
the final machine code. Debug information, as meatadata in LLVM, provides all the source level debugging
information. In LLVM this requires creating and tracking both the native type and the Debug information
metadata as independent object instances. In Llvm.NET this is handled by a unified debug and type information
system. That is, in Llvm.NET a single class is used to represent types and it acts as a binder between the
full debugging description of the type and the native LLVM minimal description. These types all implement
a common interface [ITypeRef](xref:Llvm.NET.Types.ITypeRef). This interface is used throughout Llvm.NET
to expose types in a consistent fashion. Llvm.NET provides a set of classes for building the bound types.
This sample uses the [DebugBasicType](xref:Llvm.NET.DebugInfo.DebugBasicType). To define the basic types
used in the generated code with appropriate debug information.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingBasicTypesWithDebugInfo)]

This constructs several basic types and assigns them to variables:
| Variable      | Type                  | Language Name
----------------|-----------------------|--------------
| i32           | 32 bit signed integer | int
| f32           | 32 bit IEEE Float     | float
| voidType      | void type             | n/a
| i32Array_0_32 | array i32[0..31]      | n/a
a 32 bit signed inteer type called "int" in the source language

## Creating qualified types
Creating qualified (conts, volatile, etc...) and pointers is just as easy as creating the basic types.
The sample needs a pointer to a const instance of the struct foo. A qualified type for constant foo is
created first, then a pointer type is created for the const type.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingQualifiedTypes)]

## Creating structure types
As previously mentioned, the LLVM types only contain basic layout information and not full source
level debugging information. Thus, for types there are two distinct descriptions, one for the LLVM
native type and another for the debugging information. As with basic types, Llvm.NET has support
for defining complete information for composite structure types. This is done using a collection
of [DebugMemberInfo](xref:Llvm.NET.DebugInfo.DebugMemberInfo). DebugMemberInfo fully describes an
element of a composite type including the native LLVM type as well as all the Debugging information
metadata. A collection of these is then used to create the final composite type with full debug
data in a simple single call. The sample only needs to create one such type for the `struct foo`
in the example source code.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingStructureTypes)]

## Creating module metadata and global variables
The sample code contains two global instances of `struct foo` `bar` and `baz`. Furthermore, bar
is initialized with constant data. The sample starts by constructing the const array data that
forms the initialized value of `bar.c`, the source only provides const values for the first two
entries of a 32 element array. The const data is created via [ConstArray](xref:Llvm.NET.Values.ConstantArray).
The full initialized const data for bar is the created from [Context.CreateNamedConstantStruct](xref:Llvm.NET.Context.CreateNamedConstantStruct*)

Once the constant data is available an LLVM global is created for it with a name that matches the source name
via [AddGlobal](xref:Llvm.NET.BitcodeModule.AddGlobal*). To ensure the linker lays out the structure
correctly the code uses the layout information for the module to get the ABI required alignement for 
the global and sets the [Alignment](xref:Llvm.NET.Values.GlobalObject.Alignment) property for the global.
Finally the debug information for the gloabl is created as a [DIGlobalVariableExpression](xref:Llvm.NET.DebugInfo.DIGlobalVariableExpression)
using [CreateGlobalVariableExpression](xref:Llvm.NET.DebugInfo.DebugInfoBuilder.CreateGlobalVariableExpression*)
finally the added to the variable to complete the creation.

For the `baz` instance the process is almost identical. The major difference is that the value of the
structure is initialized to all zeros. That is the initialized data for the structure is created with
[NullValueFor](xref:Llvm.NET.Values.Constant.NullValueFor*), which creates an all zero value of a type.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingGlobalsAndMetadata)]

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#AddModuleFlags)]

## Declaring the functions

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#FunctionDeclarations)]

## Generating function bodies
### Entry block

#### Arguments and debug information

### Instruction Generation

#### Intrinsic LLVM calls
