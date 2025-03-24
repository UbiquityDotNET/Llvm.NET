// -----------------------------------------------------------------------
// <copyright file="ModuleBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class ModuleBindings
    {
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LibLLVMModuleGetComdat(LLVMModuleRefAlias module, string name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt32 LibLLVMModuleGetNumComdats(LLVMModuleRefAlias module);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMComdatIteratorRef LibLLVMModuleBeginComdats(LLVMModuleRefAlias module);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static unsafe partial bool LibLLVMNextComdat(LibLLVMComdatIteratorRef it);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LibLLVMCurrentComdat(LibLLVMComdatIteratorRef it);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatIteratorReset(LibLLVMComdatIteratorRef it);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMDisposeComdatIterator(LibLLVMComdatIteratorRef it);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMGetOrInsertFunction(LLVMModuleRefAlias module, string name, LLVMTypeRef functionType);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LibLLVMGetModuleSourceFileName(LLVMModuleRefAlias module);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMSetModuleSourceFileName(LLVMModuleRefAlias module, string name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LibLLVMGetModuleName(LLVMModuleRefAlias module);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMGetGlobalAlias(LLVMModuleRefAlias module, string name);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LibLLVMModuleInsertOrUpdateComdat(LLVMModuleRefAlias module, string name, LLVMComdatSelectionKind kind);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatRemove(LLVMModuleRefAlias module, LLVMComdatRef comdatRef);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatClear(LLVMModuleRefAlias module);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof(DisposeMessageMarshaller) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LibLLVMComdatGetName(LLVMComdatRef comdatRef);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetFirstGlobalAlias(LLVMModuleRefAlias M);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetNextGlobalAlias(LLVMValueRef valueRef);
    }
}
