// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    // Native only callbacks that use a GCHandle converted to a void* as the
    // "context" to allow the native API to re-direct to the proper managed
    // implementation instance. (That is, this is a reverse P/Invoke that
    // handles marshalling of parameters and return type for native callers
    // into managed code)

    internal static class MemoryAllocatorNativeCallbacks
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe void *CreatePerObjContextAsGlobalContext(void* outerContext)
        {
            // Provide the "global"/"outer" context as the "inner"/"Per OBJ" context
            return outerContext;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe void DestroyPerObjContextNOP(void *_)
        {
            /* Intentional NOP */
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe byte* AllocateCodeSection(void* ctx, nuint size, UInt32 alignment, UInt32 sectionId, byte* sectionName)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            // NOT dispsable, just "borrowed" via ctx
            return NativeContext.TryFrom<IJitMemoryAllocator>( ctx, out var self )
                ? (byte*)self!.AllocateCodeSection(size, alignment, sectionId, LazyEncodedString.FromUnmanaged(sectionName))
                : (byte*)null;
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe byte* AllocateDataSection(
            void* ctx,
            nuint size,
            UInt32 alignment,
            UInt32 sectionId,
            byte* sectionName,
            /*LLVMBool*/Int32 isReadOnly
            )
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            // NOT dispsable, just "borrowed" via ctx
            return NativeContext.TryFrom<IJitMemoryAllocator>( ctx, out var self )
                ? (byte*)self!.AllocateDataSection(size, alignment, sectionId, LazyEncodedString.FromUnmanaged(sectionName), isReadOnly != 0)
                : (byte*)null;
#pragma warning restore CA2000 // Dispose objects before losing scope

        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe /*LLVMStatus*/ Int32 FinalizeMemory(void* ctx, byte** errMsg)
        {
            *errMsg = null;
#pragma warning disable CA2000 // Dispose objects before losing scope
            // NOT dispsable, just "borrowed" via ctx
            if(NativeContext.TryFrom<IJitMemoryAllocator>( ctx, out var self ))
            {
                if(!self!.FinalizeMemory(out LazyEncodedString? managedErrMsg))
                {
                    AllocateAndSetNativeMessage( errMsg, managedErrMsg.ToReadOnlySpan(includeTerminator: true));
                }
            }
#pragma warning restore CA2000 // Dispose objects before losing scope

            AllocateAndSetNativeMessage( errMsg, "Invalid context provided to FinalizeMemory callback!"u8);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe void NotifyTerminating(void* ctx)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            // NOT dispsable, just "borrowed" via ctx
            if(NativeContext.TryFrom<IJitMemoryAllocator>( ctx, out var self ))
            {
                // Don't dispose it here; But do release the context
                // as no more callbacks will occur after this.
                // Dispose controls the allocator handle itself, NOT the
                // context used for the callbacks. This ONLY releases the
                // callback context.
                self.ReleaseContext();
            }
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        // WARNING: Native caller ***WILL*** call `free(*errMsg)` if `*errMsg != nullptr`!! [Undocumented!]
        //          Therefore, any error message returned should be allocated with NativeMemory.Alloc()
        //          to allow free() on the pointer.
        private static unsafe void AllocateAndSetNativeMessage( byte** errMsg, ReadOnlySpan<byte> managedErrMsg )
        {
            nuint len = (nuint)managedErrMsg.Length;
            void* p = NativeMemory.Alloc(len);
            var nativeMsgSpan = new Span<byte>(p, (int)len);
            managedErrMsg.CopyTo( nativeMsgSpan );
            *errMsg = (byte*)p;
        }
    }
}
