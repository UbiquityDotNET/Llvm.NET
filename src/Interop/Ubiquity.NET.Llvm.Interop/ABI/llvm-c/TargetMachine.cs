// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    public enum LLVMCodeGenOptLevel
        : Int32
    {
        LLVMCodeGenLevelNone = 0,
        LLVMCodeGenLevelLess = 1,
        LLVMCodeGenLevelDefault = 2,
        LLVMCodeGenLevelAggressive = 3,
    }

    public enum LLVMRelocMode
        : Int32
    {
        LLVMRelocDefault = 0,
        LLVMRelocStatic = 1,
        LLVMRelocPIC = 2,
        LLVMRelocDynamicNoPic = 3,
        LLVMRelocROPI = 4,
        LLVMRelocRWPI = 5,
        LLVMRelocROPI_RWPI = 6,
    }

    public enum LLVMCodeModel
        : Int32
    {
        LLVMCodeModelDefault = 0,
        LLVMCodeModelJITDefault = 1,
        LLVMCodeModelTiny = 2,
        LLVMCodeModelSmall = 3,
        LLVMCodeModelKernel = 4,
        LLVMCodeModelMedium = 5,
        LLVMCodeModelLarge = 6,
    }

    public enum LLVMCodeGenFileType
        : Int32
    {
        LLVMAssemblyFile = 0,
        LLVMObjectFile = 1,
    }

    public enum LLVMGlobalISelAbortMode
        : Int32
    {
        LLVMGlobalISelAbortEnable = 0,
        LLVMGlobalISelAbortDisable = 1,
        LLVMGlobalISelAbortDisableWithDiag = 2,
    }

    public static partial class TargetMachine
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetFirstTarget( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetNextTarget( LLVMTargetRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetTargetFromName( LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetTargetFromTriple(
            LazyEncodedString Triple,
            out LLVMTargetRef T,
            [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetTargetName( LLVMTargetRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetTargetDescription( LLVMTargetRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasJIT( LLVMTargetRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasTargetMachine( LLVMTargetRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasAsmBackend( LLVMTargetRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineOptionsRef LLVMCreateTargetMachineOptions( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeTargetMachineOptions( LLVMTargetMachineOptionsRef Options );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCPU( LLVMTargetMachineOptionsRef Options, LazyEncodedString CPU );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetFeatures( LLVMTargetMachineOptionsRef Options, LazyEncodedString Features );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetABI( LLVMTargetMachineOptionsRef Options, LazyEncodedString ABI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCodeGenOptLevel( LLVMTargetMachineOptionsRef Options, LLVMCodeGenOptLevel Level );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetRelocMode( LLVMTargetMachineOptionsRef Options, LLVMRelocMode Reloc );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCodeModel( LLVMTargetMachineOptionsRef Options, LLVMCodeModel CodeModel );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRef LLVMCreateTargetMachineWithOptions(
            LLVMTargetRef T,
            LazyEncodedString Triple,
            LLVMTargetMachineOptionsRef Options
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRef LLVMCreateTargetMachine(
            LLVMTargetRef T,
            LazyEncodedString Triple,
            LazyEncodedString CPU,
            LazyEncodedString Features,
            LLVMCodeGenOptLevel Level,
            LLVMRelocMode Reloc,
            LLVMCodeModel CodeModel
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetTargetMachineTarget( LLVMTargetMachineRef T );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetTargetMachineTriple( LLVMTargetMachineRef T );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetTargetMachineCPU( LLVMTargetMachineRef T );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetTargetMachineFeatureString( LLVMTargetMachineRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetDataRef LLVMCreateTargetDataLayout( LLVMTargetMachineRef T );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineAsmVerbosity( LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool VerboseAsm );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineFastISel( LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineGlobalISel( LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineGlobalISelAbort( LLVMTargetMachineRef T, LLVMGlobalISelAbortMode Mode );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineMachineOutliner( LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMTargetMachineEmitToFile(
            LLVMTargetMachineRef T,
            LLVMModuleRefAlias M,
            LazyEncodedString Filename,
            LLVMCodeGenFileType codegen,
            [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage
            );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMTargetMachineEmitToMemoryBuffer(
            LLVMTargetMachineRef T,
            LLVMModuleRefAlias M,
            LLVMCodeGenFileType codegen,
            out string ErrorMessage,
            out LLVMMemoryBufferRef OutMemBuf
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetDefaultTargetTriple( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMNormalizeTargetTriple( LazyEncodedString triple );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetHostCPUName( );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMGetHostCPUFeatures( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddAnalysisPasses( LLVMTargetMachineRef T, LLVMPassManagerRef PM );
    }
}
