// -----------------------------------------------------------------------
// <copyright file="TargetMachine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LLVMCodeGenOptLevel
        : Int32
    {
        None = 0,
        Less = 1,
        Default = 2,
        Aggressive = 3,
    }

    public enum LLVMRelocMode
        : Int32
    {
        Default = 0,
        Static = 1,
        PIC = 2,
        DynamicNoPic = 3,
        ROPI = 4,
        RWPI = 5,
        ROPI_RWPI = 6,
    }

    public enum LLVMCodeModel
        : Int32
    {
        Default = 0,
        JITDefault = 1,
        Tiny = 2,
        Small = 3,
        Kernel = 4,
        Medium = 5,
        Large = 6,
    }

    public enum LLVMCodeGenFileKind
        : Int32
    {
        AssemblyFile = 0,
        ObjectFile = 1,
    }

    public enum LLVMGlobalISelAbortMode
        : Int32
    {
        Enable = 0,
        Disable = 1,
        DisableWithDiag = 2,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetFirstTarget();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetNextTarget(LLVMTargetRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetTargetFromName([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetTargetFromTriple([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Triple, out LLVMTargetRef T, [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetTargetName(LLVMTargetRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetTargetDescription(LLVMTargetRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasJIT(LLVMTargetRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasTargetMachine(LLVMTargetRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTargetHasAsmBackend(LLVMTargetRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineOptionsRef LLVMCreateTargetMachineOptions();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeTargetMachineOptions(LLVMTargetMachineOptionsRef Options);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCPU(LLVMTargetMachineOptionsRef Options, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string CPU);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetFeatures(LLVMTargetMachineOptionsRef Options, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Features);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetABI(LLVMTargetMachineOptionsRef Options, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string ABI);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCodeGenOptLevel(LLVMTargetMachineOptionsRef Options, LLVMCodeGenOptLevel Level);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetRelocMode(LLVMTargetMachineOptionsRef Options, LLVMRelocMode Reloc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMTargetMachineOptionsSetCodeModel(LLVMTargetMachineOptionsRef Options, LLVMCodeModel CodeModel);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRef LLVMCreateTargetMachineWithOptions(LLVMTargetRef T, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Triple, LLVMTargetMachineOptionsRef Options);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetMachineRef LLVMCreateTargetMachine(LLVMTargetRef T, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Triple, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string CPU, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Features, LLVMCodeGenOptLevel Level, LLVMRelocMode Reloc, LLVMCodeModel CodeModel);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetRef LLVMGetTargetMachineTarget(LLVMTargetMachineRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMGetTargetMachineTriple(LLVMTargetMachineRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMGetTargetMachineCPU(LLVMTargetMachineRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMGetTargetMachineFeatureString(LLVMTargetMachineRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetDataRef LLVMCreateTargetDataLayout(LLVMTargetMachineRef T);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineAsmVerbosity(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool VerboseAsm);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineFastISel(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineGlobalISel(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineGlobalISelAbort(LLVMTargetMachineRef T, LLVMGlobalISelAbortMode Mode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTargetMachineMachineOutliner(LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )] bool Enable);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMTargetMachineEmitToFile(LLVMTargetMachineRef T, LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Filename, LLVMCodeGenFileKind codegen, [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMTargetMachineEmitToMemoryBuffer(LLVMTargetMachineRef T, LLVMModuleRef M, LLVMCodeGenFileKind codegen, [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage, out LLVMMemoryBufferRef OutMemBuf);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMGetDefaultTargetTriple();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMNormalizeTargetTriple([MarshalUsing( typeof( AnsiStringMarshaller ) )] string triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMGetHostCPUName();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMGetHostCPUFeatures();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddAnalysisPasses(LLVMTargetMachineRef T, LLVMPassManagerRef PM);
    }
}
