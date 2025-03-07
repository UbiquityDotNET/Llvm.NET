// -----------------------------------------------------------------------
// <copyright file="PassBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Ubiquity.NET.InteropHelpers;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class NativeMethods
    {
        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMRunPasses(LLVMModuleRef M, string Passes, LLVMTargetMachineRef TM, LLVMPassBuilderOptionsRef Options);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMRunPassesOnFunction(LLVMValueRef F, string Passes, LLVMTargetMachineRef TM, LLVMPassBuilderOptionsRef Options);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassBuilderOptionsRef LLVMCreatePassBuilderOptions();

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetVerifyEach(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool VerifyEach);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetDebugLogging(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool DebugLogging);

        [LibraryImport( NativeMethods.LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetAAPipeline(LLVMPassBuilderOptionsRef Options, string AAPipeline);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopInterleaving(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopInterleaving);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopVectorization(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopVectorization);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetSLPVectorization(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool SLPVectorization);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopUnrolling(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopUnrolling);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetForgetAllSCEVInLoopUnroll(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool ForgetAllSCEVInLoopUnroll);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLicmMssaOptCap(LLVMPassBuilderOptionsRef Options, uint LicmMssaOptCap);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLicmMssaNoAccForPromotionCap(LLVMPassBuilderOptionsRef Options, uint LicmMssaNoAccForPromotionCap);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetCallGraphProfile(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool CallGraphProfile);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetMergeFunctions(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool MergeFunctions);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetInlinerThreshold(LLVMPassBuilderOptionsRef Options, int Threshold);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposePassBuilderOptions(LLVMPassBuilderOptionsRef Options);
    }
}
