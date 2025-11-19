// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly, marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be suppressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx) don't support the new syntax yet and it isn't clear if they will in the future.
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

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
