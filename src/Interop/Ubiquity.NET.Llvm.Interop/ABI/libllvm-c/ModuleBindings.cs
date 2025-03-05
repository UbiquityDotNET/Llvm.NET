// -----------------------------------------------------------------------
// <copyright file="ModuleBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LibLLVMComdatIteratorCallback = delegate* unmanaged[Cdecl]</*void**/ nint /*context*/, /*LLVMComdatRef*/ nint /*comdatRef*/, /*LLVMBool*/ Int32 /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMGetOrInsertFunction(LLVMModuleRef module, string name, LLVMTypeRef functionType);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LibLLVMGetModuleSourceFileName(LLVMModuleRef module);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMSetModuleSourceFileName(LLVMModuleRef module, string name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LibLLVMGetModuleName(LLVMModuleRef module);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMGetGlobalAlias(LLVMModuleRef module, string name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleEnumerateComdats(LLVMModuleRef module, nint context, LibLLVMComdatIteratorCallback callback);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LibLLVMModuleInsertOrUpdateComdat(LLVMModuleRef module, string name, LLVMComdatSelectionKind kind);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatRemove(LLVMModuleRef module, LLVMComdatRef comdatRef);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMModuleComdatClear(LLVMModuleRef module);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMComdatGetName(LLVMComdatRef comdatRef);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetFirstGlobalAlias(LLVMModuleRef M);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetNextGlobalAlias(LLVMValueRef valueRef);
    }
}
