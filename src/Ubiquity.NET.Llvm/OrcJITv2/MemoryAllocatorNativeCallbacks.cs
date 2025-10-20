// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    // Native only callbacks that use a SafeGCHandle as the "context" to allow
    // the native API to re-direct to the proper managed implementation instance.
    // (That is, this is a revers P/Invoke that handles marshalling of parameters
    // and return type for native callers into managed code)
    internal static class MemoryAllocatorNativeCallbacks
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe void *CreateContext(void* outerContext)
        {
            return outerContext;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe void DestroyContext(void *ctx)
        {
            /* Intentional NOP */
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe byte* AllocateCodeSection(void* ctx, nuint size, UInt32 alignment, UInt32 sectionId, byte* sectionName)
        {
            return MarshalGCHandle.TryGet<IJitMemoryAllocator>( ctx, out IJitMemoryAllocator? self )
                ? (byte*)self.AllocateCodeSection(size, alignment, sectionId, LazyEncodedString.FromUnmanaged(sectionName))
                : (byte*)null;
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
            return MarshalGCHandle.TryGet<IJitMemoryAllocator>( ctx, out IJitMemoryAllocator? self )
                ? (byte*)self.AllocateDataSection(size, alignment, sectionId, LazyEncodedString.FromUnmanaged(sectionName), isReadOnly != 0)
                : (byte*)null;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe /*LLVMStatus*/ Int32 FinalizeMemory(void* ctx, byte** errMsg)
        {
            *errMsg = null;
            if(MarshalGCHandle.TryGet<IJitMemoryAllocator>( ctx, out IJitMemoryAllocator? self ))
            {
                if(!self.FinalizeMemory(out LazyEncodedString? managedErrMsg))
                {
                    AllocateAndSetNativeMessage( errMsg, managedErrMsg.ToReadOnlySpan(includeTerminator: true));
                }
            }

            AllocateAndSetNativeMessage( errMsg, "Invalid context provided to FinalizeMemory callback!"u8);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe void DestroyMemory(void* ctx)
        {
            if(MarshalGCHandle.TryGet<IJitMemoryAllocator>( ctx, out IJitMemoryAllocator? self ))
            {
#pragma warning disable IDISP007 // Don't dispose injected
                // NOT injected; ref counted, native code is releasing ref count via this call
                self.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected
            }
        }

        // WARNING: Native caller ***WILL*** call `free(*errMsg)` if `*errMsg != nullptr`!!
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
