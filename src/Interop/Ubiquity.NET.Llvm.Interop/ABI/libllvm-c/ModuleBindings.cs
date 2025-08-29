// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class ModuleBindings
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LibLLVMModuleGetComdat( LLVMModuleRefAlias module, LazyEncodedString name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt32 LibLLVMModuleGetNumComdats( LLVMModuleRefAlias module );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMComdatIteratorRef LibLLVMModuleBeginComdats( LLVMModuleRefAlias module );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMNextComdat( LibLLVMComdatIteratorRef it );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LibLLVMCurrentComdat( LibLLVMComdatIteratorRef it );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatIteratorReset( LibLLVMComdatIteratorRef it );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMDisposeComdatIterator( LibLLVMComdatIteratorRef it );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMGetOrInsertFunction( LLVMModuleRefAlias module, LazyEncodedString name, LLVMTypeRef functionType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LibLLVMGetModuleSourceFileName( LLVMModuleRefAlias module );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMSetModuleSourceFileName( LLVMModuleRefAlias module, LazyEncodedString name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LibLLVMGetModuleName( LLVMModuleRefAlias module );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMGetGlobalAlias( LLVMModuleRefAlias module, LazyEncodedString name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LibLLVMModuleInsertOrUpdateComdat(
            LLVMModuleRefAlias module,
            LazyEncodedString name,
            LLVMComdatSelectionKind kind
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatRemove( LLVMModuleRefAlias module, LLVMComdatRef comdatRef );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatClear( LLVMModuleRefAlias module );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString LibLLVMComdatGetName( LLVMComdatRef comdatRef )
        {
            unsafe
            {
                byte* p = LibLLVMComdatGetName(comdatRef, out nuint len);
                Debug.Assert( p is not null, "Internal error: Comdat should always have a valid name" );
                return LazyEncodedString.FromUnmanaged( p, len )!;
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LibLLVMComdatGetName( LLVMComdatRef comdatRef, out nuint len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetFirstGlobalAlias( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetNextGlobalAlias( LLVMValueRef valueRef );
    }
}
