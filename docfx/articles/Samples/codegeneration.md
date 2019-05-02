# CodeGenWithDebugInfo
Sample application to generate target machine code. The sample is
provided in the [source tree](https://github.com/UbiquityDotNET/Llvm.NET/tree/master/Samples/CodeGenWithDebugInfo).

This sample generates LLVM IR equivalent to what Clang will generate for a sample C file. While it doesn't parse
the C File, this sample does show all the steps and techniques for using Llvm.NET to generate the LLVM IR with debug
information and, ultimately, the target machine code.

## Example C Code
The CodeGenWithDebugInfo sample will generate LLVM IR and machine code for the following sample "C" code.

>[!NOTE]
>The C code file is provided in the source tree along with a script file to compile it for comparing output with Clang.
>The current implementation was last compared with Clang 5 RC4 - any differences to the latest version of clang
>are expected to be minor. Updating the sample to replicate the latest Clang version is left as an exercise for
>the reader :grin:

[!code-c[Main](../../../Samples/CodeGenWithDebugInfo/Support Files/test.c)]

This sample supports targeting two different processor types x64 and ARM Cortex-M3

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
and potentially re-initialize for a different target if desired. This application only needs to generate one
module and exit so it just applies a standard C# `using` scope to ensure proper cleanup.

### Target specific details
In order to isolate the specific details of the target architecture the application uses an interface that
contains properties and methods to handle target specific support. Furthermore, an application may not need
to use all of the possible target architectures so the application selects to register/initialize support for
specific targets. This reduces startup time and resource commitments to only what is required by the application.
In this sample that is handled in the constructor of the target dependent details. Most compiler type applications
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
hidden ownership transfers that can happen with the different forms of JIT/Execution engines.
This may not always be true in future versions of the library, but for now they must be disposable.

>[!CAUTION]
>A Context is, by design, **NOT** a thread safe type. It is designed to contain various interned
objects in LLVM. All modules are owned by exactly one Context. Applications can create any
number of threads and create a context for each one. However, threads must not reference the
context of another thread nor reference any of the objects created within another thread's
context. This is a fundamental design of LLVM and reduces the complexity of attempting to
manage collections of objects and interning them in a thread safe manner. Applications instead
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
a child of that unit. The sample creates the compilation unit just after the module is created and the
target specific information is added to it. In this sample there is a direct 1:1 correlation between the
compile unit and the source file so it creates a [DIFile](xref:Llvm.NET.DebugInfo.DIFile) for the source
at the same time. The sample code creates the DICompileUnit when creating the bit code module. This is
the normal pattern for creating the compile unit when generating debugging information. Though it is possible
to create it independently and add it to the module there isn't and real benefit to doing so.

## Creating basic types with debug information
In LLVM types are fairly minimalistic and only contain the basic structural information for generating
the final machine code. Debug information, as metadata in LLVM, provides all the source level debugging
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

## Creating qualified types
Creating qualified (const, volatile, etc...) and pointers is just as easy as creating the basic types.
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

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingGlobalsAndMetadata)]

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#AddModuleFlags)]

Once the constant data is available an LLVM global is created for it with a name that matches the source name
via [AddGlobal](xref:Llvm.NET.BitcodeModule.AddGlobal*). To ensure the linker lays out the structure
correctly the code uses the layout information for the module to get the ABI required alignment for 
the global and sets the [Alignment](xref:Llvm.NET.Values.GlobalObject.Alignment) property for the global.
Finally the debug information for the global is created as a [DIGlobalVariableExpression](xref:Llvm.NET.DebugInfo.DIGlobalVariableExpression)
using [CreateGlobalVariableExpression](xref:Llvm.NET.DebugInfo.DebugInfoBuilder.CreateGlobalVariableExpression*)
finally the added to the variable to complete the creation.

For the `baz` instance the process is almost identical. The major difference is that the value of the
structure is initialized to all zeros. That is the initialized data for the structure is created with
[NullValueFor](xref:Llvm.NET.Values.Constant.NullValueFor*), which creates an all zero value of a type.

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#CreatingGlobalsAndMetadata)]

LLVM modules may contain additional module flags as metadata that describe how the module is generated
or how the code generation/linker should treat the code. In this sample the dwarf version and debug metadata
versions are set along with a VersionIdentString that identifies the application that generated the module.
Additionally, any target specific metadata is added to the module. The ordering of these is generally not
relevant, however it is very specific in the sample to help ensure the generated IR is as close to the
Clang version as possible making it possible to run llvm-dis to generate the textual IR files and compare them.
[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#AddModuleFlags)]

## Declaring the functions
The function declarations for both of the two function's is mostly the same, following a common pattern:
  1. Create the signature with debug information
  1. Create the function declaration referencing the signature
  1. Add attributes appropriate for the function

The two functions illustrate a global externally visible function and a static that is visible only locally.
This is indicated by the [Linkage.Internal](xref:Llvm.NET.Values.Linkage.Internal) linkage value.

>[!NOTE]
>The use of fluent style extension methods in the Llvm.NET API helps make it easy to add to or modify
>the attributes and linkage etc...

DeclareCopyFunc() is a bit special in that it handles some target specific support in a generalized way. In
particular the calling convention for the struct to use the `byval` form to pass the structure as a pointer
but that the callee gets a copy of the original. This, is used for some large structures and allows the target
machine generation room to use alternate means of transferring the data. (Stack or possibly otherwise unused
registers). For the two processors this sample supports Clang only uses this for the Cortex-M3 so the code
calls the TargetDetails.AddABIAttributesForByValueStructure) to add the appropriate attributes for the target
as needed. 

[!code-csharp[Main](../../../Samples/CodeGenWithDebugInfo/Program.cs#FunctionDeclarations)]

## Generating function bodies
This is where things really get interesting as this is where the actual code is generated for the functions. Up
to this point everything has created metadata or prototypes and signatures. The code generation generally follows
a pattern that starts with creation of an entry block to initialize the parameters and then additional blocks for
the actual code. While LLVM IR uses an SSA form with virtual registers, code generation, usually doesn't need to
worry about that so long as it follows some basic rules, in particular, all of the locals are allocated a slot
on the stack via alloca along with any parameters. The parameters are initialized from the signature values. All
of which is done in the entry block. LLVM has a pass (mem2reg) that will lower this into SSA form with virtual
registers so that each generating application doesn't have to worry about conversion into SSA form.

After the parameters are handled in the entry block, the rest of the function is generated based on the source
language or application defined behavior. In this case the sample generates IR equivalent to the functions defined
in the sample test.c file. There are a few points to make about the function generation in the sample.

### Generating Argument and Local variables
As discussed the arguments and locals are allocated in the entry block however that only makes them usable in
the function and ready for the mem2reg pass. In particular there is no debug information attached to the variables.
To provide debug information LLVM provides an intrinsic function that is used to declare the debug information for
a variable. In Llvm.NET this is emitted using the [InsertDeclare](xref:Llvm.NET.DebugInfo.DebugInfoBuilder.InsertDeclare*)
method.

### Calling LLVM Intrinsics
The generated code needs to copy some data, rather than directly doing a copy in a loop, the code uses the LLVM
intrinsic memcopy function. This function is lowered to an optimized copy for the target so tat applications need
not worry about building optimal versions of IR for this common functionality. Furthermore, the LLVM intrinsic
supports a variety of signatures for various data types all of which are hidden in the Llvm.NET method. Rather than
require callers to create a declaration of the correct signature the Llvm.NET wrapper automatically figures out the
correct signature from the parameters provided. 

## Final LLVM IR
```llvm
; ModuleID = 'test_M3.bc'
source_filename = "test.c"
target datalayout = "e-m:e-p:32:32-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "thumbv7m-none--eabi"

%struct.foo = type { i32, float, [32 x i32] }

@bar = global %struct.foo { i32 1, float 2.000000e+00, [32 x i32] [i32 3, i32 4, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0, i32 0] }, align 4, !dbg !0
@baz = common global %struct.foo zeroinitializer, align 4, !dbg !6

; Function Attrs: noinline nounwind optnone
define void @DoCopy() #0 !dbg !23 {
entry:
  call void @copy(%struct.foo* byval align 4 @bar, %struct.foo* @baz), !dbg !26
  ret void, !dbg !27
}

; Function Attrs: noinline nounwind optnone
define internal void @copy(%struct.foo* byval align 4 %src, %struct.foo* %pDst) #0 !dbg !28 {
entry:
  %pDst.addr = alloca %struct.foo*, align 4
  call void @llvm.dbg.declare(metadata %struct.foo* %src, metadata !33, metadata !34), !dbg !35
  store %struct.foo* %pDst, %struct.foo** %pDst.addr, align 4
  call void @llvm.dbg.declare(metadata %struct.foo** %pDst.addr, metadata !36, metadata !34), !dbg !37
  %0 = load %struct.foo*, %struct.foo** %pDst.addr, align 4, !dbg !38
  %1 = bitcast %struct.foo* %0 to i8*, !dbg !39
  %2 = bitcast %struct.foo* %src to i8*, !dbg !39
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %1, i8* %2, i32 136, i32 4, i1 false), !dbg !39
  ret void, !dbg !40
}

; Function Attrs: nounwind readnone speculatable
declare void @llvm.dbg.declare(metadata, metadata, metadata) #1

; Function Attrs: argmemonly nounwind
declare void @llvm.memcpy.p0i8.p0i8.i32(i8* nocapture writeonly, i8* nocapture readonly, i32, i32, i1) #2

attributes #0 = { noinline nounwind optnone "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "less-precise-fpmad"="false" "no-frame-pointer-elim"="true" "no-frame-pointer-elim-non-leaf" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="cortex-m3" "target-features"="+hwdiv,+strict-align,+thumb-mode" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #1 = { nounwind readnone speculatable }
attributes #2 = { argmemonly nounwind }

!llvm.dbg.cu = !{!2}
!llvm.module.flags = !{!18, !19, !20, !21}
!llvm.ident = !{!22}

!0 = !DIGlobalVariableExpression(var: !1)
!1 = distinct !DIGlobalVariable(name: "bar", scope: !2, file: !3, line: 8, type: !8, isLocal: false, isDefinition: true)
!2 = distinct !DICompileUnit(language: DW_LANG_C99, file: !3, producer: "clang version 5.0.0 (tags/RELEASE_500/rc4)", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, enums: !4, globals: !5)
!3 = !DIFile(filename: "test.c", directory: "D:\5CGitHub\5CUbiquity.NET\5CLlvm.Net\5CBuildOutput\5Cbin\5CCodeGenWithDebugInfo\5CRelease\5Cnetcoreapp2.0\5CSupport Files")
!4 = !{}
!5 = !{!0, !6}
!6 = !DIGlobalVariableExpression(var: !7)
!7 = distinct !DIGlobalVariable(name: "baz", scope: !2, file: !3, line: 9, type: !8, isLocal: false, isDefinition: true)
!8 = !DICompositeType(tag: DW_TAG_structure_type, name: "foo", file: !3, line: 1, size: 1088, elements: !9)
!9 = !{!10, !12, !14}
!10 = !DIDerivedType(tag: DW_TAG_member, name: "a", scope: !8, file: !3, line: 3, baseType: !11, size: 32)
!11 = !DIBasicType(name: "int", size: 32, encoding: DW_ATE_signed)
!12 = !DIDerivedType(tag: DW_TAG_member, name: "b", scope: !8, file: !3, line: 4, baseType: !13, size: 32, offset: 32)
!13 = !DIBasicType(name: "float", size: 32, encoding: DW_ATE_float)
!14 = !DIDerivedType(tag: DW_TAG_member, name: "c", scope: !8, file: !3, line: 5, baseType: !15, size: 1024, offset: 64)
!15 = !DICompositeType(tag: DW_TAG_array_type, baseType: !11, size: 1024, elements: !16)
!16 = !{!17}
!17 = !DISubrange(count: 32)
!18 = !{i32 2, !"Dwarf Version", i32 4}
!19 = !{i32 2, !"Debug Info Version", i32 3}
!20 = !{i32 1, !"wchar_size", i32 4}
!21 = !{i32 1, !"min_enum_size", i32 4}
!22 = !{!"clang version 5.0.0 (tags/RELEASE_500/rc4)"}
!23 = distinct !DISubprogram(name: "DoCopy", scope: !3, file: !3, line: 23, type: !24, isLocal: false, isDefinition: true, scopeLine: 24, isOptimized: false, unit: !2, variables: !4)
!24 = !DISubroutineType(types: !25)
!25 = !{null}
!26 = !DILocation(line: 25, column: 5, scope: !23)
!27 = !DILocation(line: 26, column: 1, scope: !23)
!28 = distinct !DISubprogram(name: "copy", scope: !3, file: !3, line: 11, type: !29, isLocal: true, isDefinition: true, scopeLine: 14, flags: DIFlagPrototyped, isOptimized: false, unit: !2, variables: !4)
!29 = !DISubroutineType(types: !30)
!30 = !{null, !31, !32}
!31 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !8)
!32 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !8, size: 32)
!33 = !DILocalVariable(name: "src", arg: 1, scope: !28, file: !3, line: 11, type: !31)
!34 = !DIExpression()
!35 = !DILocation(line: 11, column: 43, scope: !28)
!36 = !DILocalVariable(name: "pDst", arg: 2, scope: !28, file: !3, line: 12, type: !32)
!37 = !DILocation(line: 12, column: 38, scope: !28)
!38 = !DILocation(line: 15, column: 6, scope: !28)
!39 = !DILocation(line: 15, column: 13, scope: !28)
!40 = !DILocation(line: 16, column: 1, scope: !28)
```
