// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200, SA1135
    using unsafe LLVMMemoryManagerAllocateCodeSectionCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerAllocateDataSectionCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, /*LLVMBool*/ Int32 /*IsReadOnly*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerCreateContextCallback = delegate* unmanaged[Cdecl]< void* /*CtxCtx*/, void* /*retVal*/>;
    using unsafe LLVMMemoryManagerDestroyCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, void /*retVal*/ >;
    using unsafe LLVMMemoryManagerFinalizeMemoryCallback = delegate* unmanaged[Cdecl]< void* /*Opaque*/, byte** /*ErrMsg*/, /*LLVMBool*/ Int32 /*retVal*/>;
    using unsafe LLVMMemoryManagerNotifyTerminatingCallback = delegate* unmanaged[Cdecl]< void* /*CtxCtx*/, void /*retVal*/>;
#pragma warning restore IDE0065, SA1200, SA1135

    public static partial class OrcEE
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectLayerRef LLVMOrcCreateRTDyldObjectLinkingLayerWithSectionMemoryManager( LLVMOrcExecutionSessionRef ES );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectLayerRef LLVMOrcCreateRTDyldObjectLinkingLayerWithMCJITMemoryManagerLikeCallbacks(
            LLVMOrcExecutionSessionRef ES,
            void* CreateContextCtx,
            LLVMMemoryManagerCreateContextCallback CreateContext,
            LLVMMemoryManagerNotifyTerminatingCallback NotifyTerminating,
            LLVMMemoryManagerAllocateCodeSectionCallback AllocateCodeSection,
            LLVMMemoryManagerAllocateDataSectionCallback AllocateDataSection,
            LLVMMemoryManagerFinalizeMemoryCallback FinalizeMemory,
            LLVMMemoryManagerDestroyCallback Destroy
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcRTDyldObjectLinkingLayerRegisterJITEventListener(
            LLVMOrcObjectLayerRef RTDyldObjLinkingLayer,
            LLVMJITEventListenerRef Listener
            );
    }
}
