// -----------------------------------------------------------------------
// <copyright file="PassBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMRunPasses(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Passes, LLVMTargetMachineRef TM, LLVMPassBuilderOptionsRef Options);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMRunPassesOnFunction(LLVMValueRef F, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Passes, LLVMTargetMachineRef TM, LLVMPassBuilderOptionsRef Options);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassBuilderOptionsRef LLVMCreatePassBuilderOptions();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetVerifyEach(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool VerifyEach);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetDebugLogging(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool DebugLogging);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetAAPipeline(LLVMPassBuilderOptionsRef Options, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string AAPipeline);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopInterleaving(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopInterleaving);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopVectorization(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopVectorization);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetSLPVectorization(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool SLPVectorization);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopUnrolling(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopUnrolling);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetForgetAllSCEVInLoopUnroll(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool ForgetAllSCEVInLoopUnroll);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLicmMssaOptCap(LLVMPassBuilderOptionsRef Options, uint LicmMssaOptCap);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLicmMssaNoAccForPromotionCap(LLVMPassBuilderOptionsRef Options, uint LicmMssaNoAccForPromotionCap);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetCallGraphProfile(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool CallGraphProfile);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetMergeFunctions(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool MergeFunctions);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetInlinerThreshold(LLVMPassBuilderOptionsRef Options, int Threshold);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposePassBuilderOptions(LLVMPassBuilderOptionsRef Options);
    }
}
