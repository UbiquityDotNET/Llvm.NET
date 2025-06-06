// -----------------------------------------------------------------------
// <copyright file="DataLayoutBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class DataLayoutBindings
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LLVMErrorRef LibLLVMParseDataLayout(LazyEncodedString layoutString, out LLVMTargetDataRef outRetVal)
        {
            return LibLLVMParseDataLayout(layoutString, layoutString.NativeStrLen, out outRetVal);
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMErrorRef LibLLVMParseDataLayout(LazyEncodedString layoutString, nuint strLen, out LLVMTargetDataRef outRetVal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LazyEncodedString? LibLLVMGetDataLayoutString(LLVMTargetDataRefAlias dataLayout)
        {
            unsafe
            {
                byte* p = LibLLVMGetDataLayoutString(dataLayout, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LibLLVMGetDataLayoutString(LLVMTargetDataRefAlias dataLayout, out nuint outLen);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static unsafe partial bool LibLLVMTargeDataRefOpEquals(LLVMTargetDataRefAlias lhs, LLVMTargetDataRefAlias rhs);
    }
}
