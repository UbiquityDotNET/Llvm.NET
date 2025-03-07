// -----------------------------------------------------------------------
// <copyright file="TargetMachine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

using Ubiquity.NET.InteropHelpers;

namespace Ubiquity.NET.Llvm.Interop
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

    public static partial class NativeMethods
    {
        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetFirstTarget();

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetNextTarget(LLVMTargetRef T);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetTargetFromName(string Name);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetTargetFromTriple(string Triple, out LLVMTargetRef T, out DisposeMessageString ErrorMessage);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMGetTargetName(LLVMTargetRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMGetTargetDescription(LLVMTargetRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasJIT(LLVMTargetRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasTargetMachine(LLVMTargetRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasAsmBackend(LLVMTargetRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineOptionsRef LLVMCreateTargetMachineOptions();

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeTargetMachineOptions(LLVMTargetMachineOptionsRef Options);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCPU(LLVMTargetMachineOptionsRef Options, string CPU);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetFeatures(LLVMTargetMachineOptionsRef Options, string Features);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetABI(LLVMTargetMachineOptionsRef Options, string ABI);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCodeGenOptLevel(LLVMTargetMachineOptionsRef Options, LLVMCodeGenOptLevel Level);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetRelocMode(LLVMTargetMachineOptionsRef Options, LLVMRelocMode Reloc);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCodeModel(LLVMTargetMachineOptionsRef Options, LLVMCodeModel CodeModel);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRef LLVMCreateTargetMachineWithOptions(LLVMTargetRef T, string Triple, LLVMTargetMachineOptionsRef Options);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRef LLVMCreateTargetMachine(LLVMTargetRef T, string Triple, string CPU, string Features, LLVMCodeGenOptLevel Level, LLVMRelocMode Reloc, LLVMCodeModel CodeModel);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetTargetMachineTarget(LLVMTargetMachineRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetTargetMachineTriple(LLVMTargetMachineRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetTargetMachineCPU(LLVMTargetMachineRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetTargetMachineFeatureString(LLVMTargetMachineRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetDataRef LLVMCreateTargetDataLayout(LLVMTargetMachineRef T);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineAsmVerbosity(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool VerboseAsm);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineFastISel(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineGlobalISel(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineGlobalISelAbort(LLVMTargetMachineRef T, LLVMGlobalISelAbortMode Mode);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineMachineOutliner(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMTargetMachineEmitToFile(LLVMTargetMachineRef T, LLVMModuleRef M, string Filename, LLVMCodeGenFileType codegen, out DisposeMessageString ErrorMessage);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMTargetMachineEmitToMemoryBuffer(LLVMTargetMachineRef T, LLVMModuleRef M, LLVMCodeGenFileType codegen, out DisposeMessageString ErrorMessage, out LLVMMemoryBufferRef OutMemBuf);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetDefaultTargetTriple();

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMNormalizeTargetTriple(string triple);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetHostCPUName();

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetHostCPUFeatures();

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddAnalysisPasses(LLVMTargetMachineRef T, LLVMPassManagerRef PM);
    }
}
