// -----------------------------------------------------------------------
// <copyright file="LLJIT.cs" company="Ubiquity.NET Contributors">
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
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMOrcLLJITBuilderObjectLinkingLayerCreatorFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, nint /*LLVMOrcExecutionSessionRef ES*/, byte* /*Triple*/, /*LLVMOrcObjectLayerRef*/ nint /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcLLJITBuilderRef LLVMOrcCreateLLJITBuilder();

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeLLJITBuilder(LLVMOrcLLJITBuilderRef Builder);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcLLJITBuilderSetJITTargetMachineBuilder(LLVMOrcLLJITBuilderRef Builder, LLVMOrcJITTargetMachineBuilderRef JTMB);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcLLJITBuilderSetObjectLinkingLayerCreator(LLVMOrcLLJITBuilderRef Builder, LLVMOrcLLJITBuilderObjectLinkingLayerCreatorFunction F, void* Ctx);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateLLJIT(out LLVMOrcLLJITRef Result, LLVMOrcLLJITBuilderRef Builder);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcDisposeLLJIT(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcExecutionSessionRef LLVMOrcLLJITGetExecutionSession(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcLLJITGetMainJITDylib(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMOrcLLJITGetTripleString(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial byte LLVMOrcLLJITGetGlobalPrefix(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcLLJITMangleAndIntern(LLVMOrcLLJITRef J, string UnmangledName);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddObjectFile(LLVMOrcLLJITRef J, LLVMOrcJITDylibRef JD, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddObjectFileWithRT(LLVMOrcLLJITRef J, LLVMOrcResourceTrackerRef RT, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddLLVMIRModule(LLVMOrcLLJITRef J, LLVMOrcJITDylibRef JD, LLVMOrcThreadSafeModuleRef TSM);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITAddLLVMIRModuleWithRT(LLVMOrcLLJITRef J, LLVMOrcResourceTrackerRef JD, LLVMOrcThreadSafeModuleRef TSM);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcLLJITLookup(LLVMOrcLLJITRef J, out UInt64 Result, string Name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectLayerRef LLVMOrcLLJITGetObjLinkingLayer(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectTransformLayerRef LLVMOrcLLJITGetObjTransformLayer(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcIRTransformLayerRef LLVMOrcLLJITGetIRTransformLayer(LLVMOrcLLJITRef J);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMOrcLLJITGetDataLayoutStr(LLVMOrcLLJITRef J);
    }
}
