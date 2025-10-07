// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Extensions;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Extensions for <see cref="LLVMOrcSymbolStringPoolEntryRef"/></summary>
    /// <remarks>These are extension methods to allow for consistent handle generation.</remarks>
    public static class SymbolStringPoolEntryRefExtension
    {
#if DEBUG // Access to the ref count is an inherently unstable aspect and only useful for debug/diagnosis purposes
        public static nuint GetRefCount(this LLVMOrcSymbolStringPoolEntryRef self)
        {
            return LibLLVMOrcSymbolStringPoolGetRefCount(self.DangerousGetHandle());
        }
#endif

        /// <summary>Performs an addref on the native instance</summary>
        /// <param name="self">Handle of the native string</param>
        /// <returns>New handle that owns the addref</returns>
        [DebuggerStepThrough]
        [MustUseReturnValue]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LLVMOrcSymbolStringPoolEntryRef NativeAddRef(this LLVMOrcSymbolStringPoolEntryRef self)
        {
            nint abiHandle = self.DangerousGetHandle();
            LLVMOrcRetainSymbolStringPoolEntry(abiHandle);
            return LLVMOrcSymbolStringPoolEntryRef.FromABI(abiHandle);
        }

        /// <summary>Performs a decrement of the ref count (Release) on the native instance</summary>
        /// <param name="self">Handle of the native string</param>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NativeRelease(this LLVMOrcSymbolStringPoolEntryRef self)
        {
            LLVMOrcReleaseSymbolStringPoolEntry(self.DangerousGetHandle());
        }

        /// <summary>Performs an addref on the native instance</summary>
        /// <param name="self">Handle of the native string</param>
        /// <returns>New handle that owns the addref</returns>
        [DebuggerStepThrough]
        [MustUseReturnValue]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint AddRefToNative(this LLVMOrcSymbolStringPoolEntryRef self)
        {
            nint abiHandle = self.DangerousGetHandle();
            LLVMOrcRetainSymbolStringPoolEntry(abiHandle);
            return abiHandle;
        }

        /// <summary>Performs an addref on the native instance</summary>
        /// <param name="self">Handle of the native string</param>
        /// <returns>New handle that owns the addref</returns>
        [DebuggerStepThrough]
        [MustUseReturnValue]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LLVMOrcSymbolStringPoolEntryRef NativeAddRef(this LLVMOrcSymbolStringPoolEntryRefAlias self)
        {
            nint abiHandle = self.DangerousGetHandle();
            LLVMOrcRetainSymbolStringPoolEntry(abiHandle);
            return LLVMOrcSymbolStringPoolEntryRef.FromABI(abiHandle);
        }

        /// <summary>Performs a decrement of the ref count (Release) on the native instance</summary>
        /// <param name="self">Handle of the native string</param>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NativeRelease(this LLVMOrcSymbolStringPoolEntryRefAlias self)
        {
            LLVMOrcReleaseSymbolStringPoolEntry(self.DangerousGetHandle());
        }

        // Marshalling for these is NOT needed, or desired, it's the lowest level of the API call from managed code
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
        [DllImport( LibraryName )]
        [SuppressGCTransition]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static extern void LLVMOrcRetainSymbolStringPoolEntry( /*LLVMOrcSymbolStringPoolEntryRef*/ nint h );

        [DllImport( LibraryName )]
        [SuppressGCTransition]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static extern void LLVMOrcReleaseSymbolStringPoolEntry( /*LLVMOrcSymbolStringPoolEntryRef*/ nint h );

#if DEBUG // Access to the ref count is an inherently unstable aspect and only useful for debug/diagnosis purposes
        [DllImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static extern /*size_t*/ nuint LibLLVMOrcSymbolStringPoolGetRefCount(/*LLVMOrcSymbolStringPoolEntryRef*/ nint h);
#endif

#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
    }
}
