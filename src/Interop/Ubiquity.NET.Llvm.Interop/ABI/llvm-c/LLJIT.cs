// -----------------------------------------------------------------------
// <copyright file="LLJIT.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMOrcLLJITBuilderObjectLinkingLayerCreatorFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, nint /*LLVMOrcExecutionSessionRef ES*/, byte* /*Triple*/, /*LLVMOrcObjectLayerRef*/ nint /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    public static partial class LLJIT
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcLLJITBuilderRef LLVMOrcCreateLLJITBuilder( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeLLJITBuilder( LLVMOrcLLJITBuilderRef Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcLLJITBuilderSetJITTargetMachineBuilder(
            LLVMOrcLLJITBuilderRef Builder,
            LLVMOrcJITTargetMachineBuilderRef JTMB
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcLLJITBuilderSetObjectLinkingLayerCreator(
            LLVMOrcLLJITBuilderRef Builder,
            LLVMOrcLLJITBuilderObjectLinkingLayerCreatorFunction F,
            void* Ctx
            );

        // simple wrapper to re-order parameters for inconsistent API design
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMErrorRef LLVMOrcCreateLLJIT(
            LLVMOrcLLJITBuilderRef Builder,
            /*[MaybeInvalidWhen(Failed)]*/ out LLVMOrcLLJITRef Result
            )
        {
            return LLVMOrcCreateLLJIT( out Result, Builder );
        }

        // NOTE: Confusingly backwards API design, out is first param, and handle is last!
        // This is inconsistent with standard practice and even LLVM APIs
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMErrorRef LLVMOrcCreateLLJIT(
            /*[MaybeInvalidWhen(Failed)]*/ out LLVMOrcLLJITRef Result,
            LLVMOrcLLJITBuilderRef Builder
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcDisposeLLJIT( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcExecutionSessionRef LLVMOrcLLJITGetExecutionSession( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcLLJITGetMainJITDylib( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMOrcLLJITGetTripleString( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial byte LLVMOrcLLJITGetGlobalPrefix( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcLLJITMangleAndIntern( LLVMOrcLLJITRef J, LazyEncodedString UnmangledName );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddObjectFile( LLVMOrcLLJITRef J, LLVMOrcJITDylibRef JD, LLVMMemoryBufferRef ObjBuffer );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddObjectFileWithRT(
            LLVMOrcLLJITRef J,
            LLVMOrcResourceTrackerRef RT,
            LLVMMemoryBufferRef ObjBuffer
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddLLVMIRModule( LLVMOrcLLJITRef J, LLVMOrcJITDylibRef JD, LLVMOrcThreadSafeModuleRef TSM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddLLVMIRModuleWithRT(
            LLVMOrcLLJITRef J,
            LLVMOrcResourceTrackerRef JD,
            LLVMOrcThreadSafeModuleRef TSM
            );

        // BAD API design, out result should be last parameter... but isn't.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITLookup( LLVMOrcLLJITRef J, out UInt64 Result, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectLayerRef LLVMOrcLLJITGetObjLinkingLayer( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectTransformLayerRef LLVMOrcLLJITGetObjTransformLayer( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcIRTransformLayerRef LLVMOrcLLJITGetIRTransformLayer( LLVMOrcLLJITRef J );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMOrcLLJITGetDataLayoutStr( LLVMOrcLLJITRef J );
    }
}
