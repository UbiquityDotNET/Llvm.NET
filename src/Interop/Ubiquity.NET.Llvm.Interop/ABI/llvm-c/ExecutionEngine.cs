// -----------------------------------------------------------------------
// <copyright file="ExecutionEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMMemoryManagerAllocateCodeSectionCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerAllocateDataSectionCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, bool /*IsReadOnly*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerDestroyCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, void /*retVal*/ >;
    using unsafe LLVMMemoryManagerFinalizeMemoryCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, byte** /*ErrMsg*/, bool /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMMCJITCompilerOptions
    {
        public readonly uint OptLevel;
        public readonly LLVMCodeModel CodeModel;
        public readonly UInt32 /*LLVMBool*/ NoFramePointerElim;
        public readonly UInt32 /*LLVMBool*/ EnableFastISel;
        public readonly nint /*LLVMMCJITMemoryManagerRef*/ MCJMM;
    }

    public static partial class NativeMethods
    {
        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMLinkInMCJIT();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMLinkInInterpreter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMCreateGenericValueOfInt(LLVMTypeRef Ty, ulong N, [MarshalAs( UnmanagedType.Bool )] bool IsSigned);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMCreateGenericValueOfPointer(void* P);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMCreateGenericValueOfFloat(LLVMTypeRef Ty, double N);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGenericValueIntWidth(LLVMGenericValueRef GenValRef);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMGenericValueToInt(LLVMGenericValueRef GenVal, [MarshalAs( UnmanagedType.Bool )] bool IsSigned);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGenericValueToPointer(LLVMGenericValueRef GenVal);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial double LLVMGenericValueToFloat(LLVMTypeRef TyRef, LLVMGenericValueRef GenVal);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateExecutionEngineForModule(out LLVMExecutionEngineRef OutEE, LLVMModuleRef M, out DisposeMessageString OutError);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateInterpreterForModule(out LLVMExecutionEngineRef OutInterp, LLVMModuleRef M, out DisposeMessageString OutError);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateJITCompilerForModule(out LLVMExecutionEngineRef OutJIT, LLVMModuleRef M, uint OptLevel, out DisposeMessageString OutError);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMCJITCompilerOptions(out LLVMMCJITCompilerOptions Options, size_t SizeOfOptions);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateMCJITCompilerForModule(out LLVMExecutionEngineRef OutJIT, LLVMModuleRef M, out global::Ubiquity.NET.Llvm.Interop.LLVMMCJITCompilerOptions Options, size_t SizeOfOptions, out DisposeMessageString OutError);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRunStaticConstructors(LLVMExecutionEngineRef EE);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRunStaticDestructors(LLVMExecutionEngineRef EE);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMRunFunctionAsMain(LLVMExecutionEngineRef EE, LLVMValueRef F, uint ArgC, [In][MarshalAs( UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr )] string[] ArgV, [In][MarshalAs( UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr )] string[] EnvP);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMRunFunction(LLVMExecutionEngineRef EE, LLVMValueRef F, uint NumArgs, out LLVMGenericValueRef Args);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMFreeMachineCodeForFunction(LLVMExecutionEngineRef EE, LLVMValueRef F);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddModule(LLVMExecutionEngineRef EE, LLVMModuleRef M);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMRemoveModule(LLVMExecutionEngineRef EE, LLVMModuleRef M, out LLVMModuleRef OutMod, out DisposeMessageString OutError);

        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMFindFunction(LLVMExecutionEngineRef EE, string Name, out LLVMValueRef OutFn);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMRecompileAndRelinkFunction(LLVMExecutionEngineRef EE, LLVMValueRef Fn);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetDataRefAlias LLVMGetExecutionEngineTargetData(LLVMExecutionEngineRef EE);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRefAlias LLVMGetExecutionEngineTargetMachine(LLVMExecutionEngineRef EE);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddGlobalMapping(LLVMExecutionEngineRef EE, LLVMValueRef Global, void* Addr);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGetPointerToGlobal(LLVMExecutionEngineRef EE, LLVMValueRef Global);

        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetGlobalValueAddress(LLVMExecutionEngineRef EE, string Name);

        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetFunctionAddress(LLVMExecutionEngineRef EE, string Name);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMExecutionEngineGetErrMsg(LLVMExecutionEngineRef EE, out DisposeMessageString OutError);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMCJITMemoryManagerRef LLVMCreateSimpleMCJITMemoryManager(void* Opaque, LLVMMemoryManagerAllocateCodeSectionCallback AllocateCodeSection, LLVMMemoryManagerAllocateDataSectionCallback AllocateDataSection, LLVMMemoryManagerFinalizeMemoryCallback FinalizeMemory, LLVMMemoryManagerDestroyCallback Destroy);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreateGDBRegistrationListener();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreateIntelJITEventListener();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreateOProfileJITEventListener();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreatePerfJITEventListener();
    }
}
