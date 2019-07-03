# LLVM 8.0 Support
## Llvm.NET.Interop (New library)
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

### Auto-generated P/Invoke
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

## New features
* ObjectFile Support
  * LLvm.NET.ObjectFile namespace contains support for processing object files using LLVM
* Eager compilation JIT
  * The OrcJIT now supports eager and lazy compilation for Windows platforms
* Full initialization for all the latests supported targets
  * Including - BPF, Lanai, WebAssembly, MSP430, NVPTX, AMDGPU, Hexagon, and XCore
* Added accessors to allow retrieval/addition of metadata on instructions

# Breaking Changes
This is a Major release and, as such, can, and does, have breaking changes including:

1. New namespace for some classes (Llvm.NET.Interop)
2. StaticState class is renamed to Llvm.NET.Interop.Library as it is fundamentally 
   part of the low level interop (and "StaticState" was always a bad name)
3. Instructions no longer have a SetDebugLocation, instead that is provided via a new
   fluent method on the InstructionBuilder since the normal use is to set the location
   on the builder and then generate a sequence of IR instructions for a given expression
   in code. 
4. Legacy JIT engine support is dropped. ORCJit is the only supported JIT engine
5. Context.CreateBasicBlock() now only creates detached blocks, if append to a function
   is desired, there is a method on Function to create and append a new block.
6. PassManager, ModulePassManager, and FunctionPassManager are IDisposable to help apps
   ensure that a function pass manager, which is bound to a module, is destroyed before
   the module it is bound to.
7. BitcodeModule.MakeShared and shared refs of modules is removed. (This was created for
   OrcJIT use of shared_ptr under the hood, which is no longer used. OrcJit now uses the
   same ownership transfer model as the legacy engines. E.g. the ownership for the module
   is transferred to the JIT engine)
8. BitCodeModule is now Disposable backed by a safe handle, this allows for detaching and
   invalidating the underlying LLVMModuleRef when the module is provided to the JIT
9. Renamed Function class to IrFunction to avoid potential collision with common language
   keywords
10. Renamed Select to SelectInstruction to avoid potential collision with language keyword
    and make consistent with ReturnInstruction, ResumeInstruction and other similar cases
    for instructions.

