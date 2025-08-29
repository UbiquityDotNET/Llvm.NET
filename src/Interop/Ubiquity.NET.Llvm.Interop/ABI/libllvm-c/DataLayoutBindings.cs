// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class DataLayoutBindings
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMErrorRef LibLLVMParseDataLayout( LazyEncodedString layoutString, out LLVMTargetDataRef outRetVal )
        {
            return LibLLVMParseDataLayout( layoutString, layoutString.NativeStrLen, out outRetVal );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMErrorRef LibLLVMParseDataLayout( LazyEncodedString layoutString, nuint strLen, out LLVMTargetDataRef outRetVal );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LibLLVMGetDataLayoutString( LLVMTargetDataRefAlias dataLayout )
        {
            unsafe
            {
                byte* p = LibLLVMGetDataLayoutString(dataLayout, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LibLLVMGetDataLayoutString( LLVMTargetDataRefAlias dataLayout, out nuint outLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMTargeDataRefOpEquals( LLVMTargetDataRefAlias lhs, LLVMTargetDataRefAlias rhs );
    }
}
