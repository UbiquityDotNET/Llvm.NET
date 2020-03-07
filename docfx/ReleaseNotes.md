# Release Notes
## v8.0.1
### Bug Fixes

| Bug | Description |
|-------|--------------|
| [151](https://github.com/UbiquityDotNET/Llvm.NET/issues/151) | Updated DebugFunctionType signature to use interface instead of concrete type |
| [152](https://github.com/UbiquityDotNET/Llvm.NET/issues/152) | Corrected docs copy/paste error [renaming part of the issue is left for the next major release as that is a breaking change] |

### Additional changes
Additionally the internal build scripts were updated to simplify the consistent corss solution versioning. Previously,
a complex process of building a dummy project to generate a data file was used, however that was no longer necessary
as the [CSemVer.Build.Tasks ](https://github.com/UbiquityDotNET/CSemVer.GitBuild) package can figure out all except
the CiBuildIndex, which, for this project, is an ISO-8601 formatted timestamp (of the latest commit for automated
builds or the build start for local developer builds)

## v8.0.0
### Llvm.NET.Interop (New library)
Llvm.NET 8.0 adds a new library (Llvm.NET.Interop)  that contains the raw P/Invoke
APIs and support needed to inter-operate with the native library. The NuGet package
for the interop library includes the native code binaries as they are tightly coupled.
This package contains the native LibLLVM.dll and the P/Invoke interop support layers.
Llvm.NET uses this library to define a clean projection of LLVM for .NET consumers.
This will, hopefully, allow for future development and enhancement of the Llvm.NET
object model without changing the underlying P/Invoke layers. (e.g.
the Llvm.NET.Interop can "snap" to LLVM versions, but the LLvm.NET model can have
multiple incremental releases) This isn't a hard/fast rule as it is possible that
getting new functionality in the object model requires new custom extensions. At
this point in time both libraries are built together and share build numbers.
Though, that may change in the future. 

#### Auto-generated P/Invoke
LLVM-C API now includes most of the debug APIs so, significantly fewer custom
extensions are needed (That's a good thing!). To try and keep things simpler this
moves the interop back to using code generation for the bulk of the P/Invoke interop.
However, unlike the first use of generation, the [LLVMBindingsGenerator](https://github.com/UbiquityDotNET/Llvm.NET/tree/master/src/Interop/LlvmBindingsGenerator)
is much more targeted and includes specialized handling to prevent the need for
additional "by-hand" tweaking of the generated code, such as:

1. Marshaling of strings with the many ways to dispose (or not) a returned string
2. LLVMBool vs LLVMStatus
3. "smart ref" handle types, including aliases that should not be released by
   client code.

The generated code is combined with some fixed support classes to create a new
Llvm.NET.Interop Library and NuGet Package. 

### New features
* ObjectFile Support
  * LLvm.NET.ObjectFile namespace contains support for processing object files using LLVM
* Eager compilation JIT
  * The OrcJIT now supports eager and lazy compilation for Windows platforms
* Full initialization for all the latests supported targets
  * Including - BPF, Lanai, WebAssembly, MSP430, NVPTX, AMDGPU, Hexagon, and XCore
* Added accessors to allow retrieval/addition of metadata on instructions

### Breaking Changes
This is a Major release and, as such, can, and does, have breaking changes. While there
are several such changes the actual impact to a code base is fairly trivial. Most are
driven by either obsolescence of functionality in LLVM or general naming cleanup in the
Llvm.NET library:

1. New namespace and assembly for some classes (Llvm.NET.Interop)
    1. Llvm.NET.DisposableAction -> Llvm.NET.Interop.DisposableAction
    2. Llvm.NET.DisposableObject -> Llvm.NET.Interop.DisposableObject
    3. Llvm.NET.StaticState -> Llvm.NET.Interop.Library
    4. Llvm.NET.TargetRegistrations -> Llvm.NET.Interop.TargetRegistrations
2. StaticState class is renamed to Llvm.NET.Interop.Library as it is fundamentally 
   part of the low level interop (and "StaticState" was always a bad name)
3. Instructions no longer have a SetDebugLocation, instead that is provided via a new
   fluent method on the InstructionBuilder since the normal use is to set the location
   on the builder and then generate a sequence of IR instructions for a given expression
   in code. 
4. Legacy JIT engine support is dropped. ORCJit is the only supported JIT engine
    1. Removed Llvm.NET.JIT.EngineKind
    2. Removed all use of Llvm.NET.JIT.IJitModuleHandle. Handles are now just an integral value
    3. Removed Llvm.NET.LegacyExecutionEngine
5. Context.CreateBasicBlock() now only creates detached blocks, if append to a function
   is desired, there is a method on IrFunction to create and append a block.
    1. CreateBasicBlock signature changed to remove the function and block parameters 
6. PassManager, ModulePassManager, and FunctionPassManager are IDisposable to help apps
   ensure that a function pass manager, which is bound to a module, is destroyed before
   the module it is bound to. Failure to do so can result in app crashes from access
   violations in the native LLVM code.
7. BitcodeModule
    1. MakeShared and shared refs of modules is removed. (This was created for
        OrcJIT use of shared_ptr under the hood, which is no longer used. OrcJit now uses the
        same ownership transfer model as the legacy engines. E.g. the ownership for the module
        is transferred to the JIT engine)
    2. BitCodeModule is now Disposable backed by a safe handle, this allows for detaching and
       invalidating the underlying LLVMModuleRef when the module is provided to the JIT
    3. CreateFunction() signature changed, Dropped the default null node parameters
       not supported by the LLVM-C implementation.

8. Renamed Function class to IrFunction to avoid potential collision with common language
   keywords
9. Renamed Select to SelectInstruction to avoid potential collision with language keyword
    and make consistent with ReturnInstruction, ResumeInstruction and other similar cases
    for instructions.
10. Removed transform pass functions not supported in LLVM-C
    1. SclaraTransforms.AddLateCFGSimplificationPass()
11. `GlobalValueExtensions.UnnameAddress<T>(T,bool)` was changed to
    `GlobalValueExtensions.UnnameAddress<T>(T,UnnamedAddressKind)` to support changes in
    underlying LLVM
12. Removed ValueExtensions.SetDugLocation() [All overloaded forms], debug location is set
    in the InstructionBuilder and remains in effect for all instructions until reset or
    cleared by setting it to null.
13. DIBuilder
    1. CreateFunction() signature changed, Dropped the default null node parameters
       not supported by the LLVM-C implementation.
    2. DIBuilder.CreateReplaceableCompositeType() and CreateUnionType() signatures changed to
       include unique ID
       1. The id is set to default to string.Empty so this should largely go without actually
          breaking anything
    3. CreateBasicType Added DebugIngoFlags parameter
    4. CreateEnumerationType removed uniqueId string parameter as it isn't supported by LLVM-C
    5. Obsoleted CreateStructType signature taking `DINodeArray` in favor of `IEnumerable<DINode>`
14. Llvm.NET.DebugInfo.ExpressionOp names changed to correct PascalCasing and eliminate
    underscores in the value names for better consistency and style compliance.
15. Renamed some Llvm.NET.DebugInfo.SourceLanguage vendor specific values to conform with
    underlying LLVM names
    1. RenderScript -> GoogleRenderScript
    2. Delphi -> BorlandDelphi
16. Renamed or removed some of the Llvm.NET.DebugInfo.Tag values to better reflect underlying
    LLVM names and avoid potential language keyword conflicts.
    1. Label -> TagLabel
    2. PtrToMemberType -> PointerToMemberType
    3. Removed AutoVariable, ArgVariable, Expression, UserBase, LoUser and MipsLoop as they
       don't exist in the LLVM support.
17. InstructionBuilder
    1. Obsoleted Methods that don't support opaque pointers in preparation for LLVM's transition
    2. Changed MemCpy, MemMove, and MemSet signatures to remove alignment as LLVM intrinsic no
       longer includes an alignment parameter. It is applied as a parameter attribute for each 
       of the pointer parameters (source and destination).
18. Llvm.NET.JIT.IExecutionEngine
    1. Replaced AddModule with AddEagerlyCompiledModule to make it more explicit on the behavior
19. Llvm.NET.ILazyCompileExecutionEngine
    1. Replaced AddModule [From IExecutionEngine] with AddLazyCompiledModule to make it explicit
    2. Removed DefalultSymbolResolver from interface as it should not have been in the interface
       to start with.
20. Deleted Llvm.NET.LegacyExecutionEngine
21. Llvm.NET.JIT.OrcJit - updated to reflect changes in the IExecutionEngine and
    ILazyCompileExecutionEngine interfaces.

