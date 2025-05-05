// -----------------------------------------------------------------------
// <copyright file="PassBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    public static partial class PassBuilder
    {
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMRunPasses(LLVMModuleRefAlias M, string Passes, LLVMTargetMachineRef TM, LLVMPassBuilderOptionsRef Options);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMRunPassesOnFunction(LLVMValueRef F, string Passes, LLVMTargetMachineRef TM, LLVMPassBuilderOptionsRef Options);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassBuilderOptionsRef LLVMCreatePassBuilderOptions();

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetVerifyEach(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool VerifyEach);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetDebugLogging(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool DebugLogging);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetAAPipeline(LLVMPassBuilderOptionsRef Options, string AAPipeline);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopInterleaving(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopInterleaving);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopVectorization(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopVectorization);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetSLPVectorization(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool SLPVectorization);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLoopUnrolling(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool LoopUnrolling);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetForgetAllSCEVInLoopUnroll(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool ForgetAllSCEVInLoopUnroll);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLicmMssaOptCap(LLVMPassBuilderOptionsRef Options, uint LicmMssaOptCap);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetLicmMssaNoAccForPromotionCap(LLVMPassBuilderOptionsRef Options, uint LicmMssaNoAccForPromotionCap);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetCallGraphProfile(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool CallGraphProfile);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetMergeFunctions(LLVMPassBuilderOptionsRef Options, [MarshalAs( UnmanagedType.Bool )] bool MergeFunctions);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPassBuilderOptionsSetInlinerThreshold(LLVMPassBuilderOptionsRef Options, int Threshold);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposePassBuilderOptions(LLVMPassBuilderOptionsRef Options);
    }
}
