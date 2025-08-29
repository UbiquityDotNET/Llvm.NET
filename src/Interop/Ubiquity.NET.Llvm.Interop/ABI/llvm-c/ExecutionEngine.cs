// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200, SA1135
    using unsafe LLVMMemoryManagerAllocateCodeSectionCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerAllocateDataSectionCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, bool /*IsReadOnly*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerDestroyCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, void /*retVal*/ >;
    using unsafe LLVMMemoryManagerFinalizeMemoryCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, byte** /*ErrMsg*/, bool /*retVal*/>;
#pragma warning restore IDE0065, SA1200, SA1135

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMMCJITCompilerOptions
    {
        public readonly uint OptLevel;
        public readonly LLVMCodeModel CodeModel;
        public readonly UInt32 /*LLVMBool*/ NoFramePointerElim;
        public readonly UInt32 /*LLVMBool*/ EnableFastISel;
        public readonly nint /*LLVMMCJITMemoryManagerRef*/ MCJMM;
    }

    public static partial class ExecutionEngine
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMLinkInMCJIT( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMLinkInInterpreter( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMCreateGenericValueOfInt(
            LLVMTypeRef Ty,
            ulong N,
            [MarshalAs( UnmanagedType.Bool )] bool IsSigned
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMCreateGenericValueOfPointer( void* P );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMCreateGenericValueOfFloat( LLVMTypeRef Ty, double N );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGenericValueIntWidth( LLVMGenericValueRef GenValRef );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMGenericValueToInt( LLVMGenericValueRef GenVal, [MarshalAs( UnmanagedType.Bool )] bool IsSigned );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGenericValueToPointer( LLVMGenericValueRef GenVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial double LLVMGenericValueToFloat( LLVMTypeRef TyRef, LLVMGenericValueRef GenVal );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateExecutionEngineForModule(
            out LLVMExecutionEngineRef OutEE,
            LLVMModuleRef M,
            out string OutError
            );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateInterpreterForModule(
            out LLVMExecutionEngineRef OutInterp,
            LLVMModuleRef M,
            out string OutError
            );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateJITCompilerForModule(
            out LLVMExecutionEngineRef OutJIT,
            LLVMModuleRef M,
            uint OptLevel,
            out string OutError
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void LLVMInitializeMCJITCompilerOptions( out LLVMMCJITCompilerOptions Options )
        {
            LLVMInitializeMCJITCompilerOptions( out Options, checked((nuint)Unsafe.SizeOf<LLVMMCJITCompilerOptions>()) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMInitializeMCJITCompilerOptions( out LLVMMCJITCompilerOptions Options, nuint SizeOfOptions );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMStatus LLVMCreateMCJITCompilerForModule(
            out LLVMExecutionEngineRef OutJIT,
            LLVMModuleRef M,
            out LLVMMCJITCompilerOptions Options,
            out string OutError
            )
        {
            return LLVMCreateMCJITCompilerForModule(
                out OutJIT,
                M,
                out Options,
                checked((nuint)Unsafe.SizeOf<LLVMMCJITCompilerOptions>()),
                out OutError
            );
        }

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMStatus LLVMCreateMCJITCompilerForModule(
            out LLVMExecutionEngineRef OutJIT,
            LLVMModuleRef M,
            out LLVMMCJITCompilerOptions Options,
            nuint SizeOfOptions,
            out string OutError
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRunStaticConstructors( LLVMExecutionEngineRef EE );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRunStaticDestructors( LLVMExecutionEngineRef EE );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMRunFunctionAsMain(
            LLVMExecutionEngineRef EE,
            LLVMValueRef F,
            uint ArgC,
            [In][MarshalAs( UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr )] string[] ArgV,
            [In][MarshalAs( UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr )] string[] EnvP
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGenericValueRef LLVMRunFunction(
            LLVMExecutionEngineRef EE,
            LLVMValueRef F,
            uint NumArgs,
            out LLVMGenericValueRef Args
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMFreeMachineCodeForFunction( LLVMExecutionEngineRef EE, LLVMValueRef F );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddModule( LLVMExecutionEngineRef EE, LLVMModuleRef M );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMRemoveModule(
            LLVMExecutionEngineRef EE,
            LLVMModuleRef M,
            out LLVMModuleRef OutMod,
            out string OutError
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMFindFunction( LLVMExecutionEngineRef EE, LazyEncodedString Name, out LLVMValueRef OutFn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMRecompileAndRelinkFunction( LLVMExecutionEngineRef EE, LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetDataRefAlias LLVMGetExecutionEngineTargetData( LLVMExecutionEngineRef EE );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRefAlias LLVMGetExecutionEngineTargetMachine( LLVMExecutionEngineRef EE );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddGlobalMapping( LLVMExecutionEngineRef EE, LLVMValueRef Global, void* Addr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGetPointerToGlobal( LLVMExecutionEngineRef EE, LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetGlobalValueAddress( LLVMExecutionEngineRef EE, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetFunctionAddress( LLVMExecutionEngineRef EE, LazyEncodedString Name );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMExecutionEngineGetErrMsg( LLVMExecutionEngineRef EE, out string OutError );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMCJITMemoryManagerRef LLVMCreateSimpleMCJITMemoryManager(
            void* Opaque,
            LLVMMemoryManagerAllocateCodeSectionCallback AllocateCodeSection,
            LLVMMemoryManagerAllocateDataSectionCallback AllocateDataSection,
            LLVMMemoryManagerFinalizeMemoryCallback FinalizeMemory,
            LLVMMemoryManagerDestroyCallback Destroy
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreateGDBRegistrationListener( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreateIntelJITEventListener( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreateOProfileJITEventListener( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMJITEventListenerRef LLVMCreatePerfJITEventListener( );
    }
}
