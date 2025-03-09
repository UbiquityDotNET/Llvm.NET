// -----------------------------------------------------------------------
// <copyright file="OrcEE.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMMemoryManagerAllocateCodeSectionCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerAllocateDataSectionCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, nuint /*Size*/, uint /*Alignment*/, uint /*SectionID*/, byte* /*SectionName*/, bool /*IsReadOnly*/, byte* /*retVal*/>;
    using unsafe LLVMMemoryManagerCreateContextCallback = delegate* unmanaged[Cdecl]<void* /*CtxCtx*/, void* /*retVal*/>;
    using unsafe LLVMMemoryManagerDestroyCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, void /*retVal*/ >;
    using unsafe LLVMMemoryManagerFinalizeMemoryCallback = delegate* unmanaged[Cdecl]<void* /*Opaque*/, byte** /*ErrMsg*/, bool /*retVal*/>;
    using unsafe LLVMMemoryManagerNotifyTerminatingCallback = delegate* unmanaged[Cdecl]<void* /*CtxCtx*/, void /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    public static partial class OrcEE
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectLayerRef LLVMOrcCreateRTDyldObjectLinkingLayerWithSectionMemoryManager(LLVMOrcExecutionSessionRef ES);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcObjectLayerRef LLVMOrcCreateRTDyldObjectLinkingLayerWithMCJITMemoryManagerLikeCallbacks(LLVMOrcExecutionSessionRef ES, void* CreateContextCtx, LLVMMemoryManagerCreateContextCallback CreateContext, LLVMMemoryManagerNotifyTerminatingCallback NotifyTerminating, LLVMMemoryManagerAllocateCodeSectionCallback AllocateCodeSection, LLVMMemoryManagerAllocateDataSectionCallback AllocateDataSection, LLVMMemoryManagerFinalizeMemoryCallback FinalizeMemory, LLVMMemoryManagerDestroyCallback Destroy);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcRTDyldObjectLinkingLayerRegisterJITEventListener(LLVMOrcObjectLayerRef RTDyldObjLinkingLayer, LLVMJITEventListenerRef Listener);
    }
}
