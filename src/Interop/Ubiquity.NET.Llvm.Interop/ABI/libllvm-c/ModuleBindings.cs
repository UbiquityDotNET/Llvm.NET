// -----------------------------------------------------------------------
// <copyright file="ModuleBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
// Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LibLLVMComdatIteratorCallback = delegate* <LLVMComdatRef /*comdatRef*/, bool /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMValueRef LibLLVMGetOrInsertFunction( LLVMModuleRef module, [MarshalUsing( typeof(AnsiStringMarshaller) )]string name, LLVMTypeRef functionType );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LibLLVMGetModuleSourceFileName( LLVMModuleRef module );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMSetModuleSourceFileName( LLVMModuleRef module, [MarshalUsing( typeof(AnsiStringMarshaller) )]string name );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LibLLVMGetModuleName( LLVMModuleRef module );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMValueRef LibLLVMGetGlobalAlias( LLVMModuleRef module, [MarshalUsing( typeof(AnsiStringMarshaller) )]string name );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMModuleEnumerateComdats( LLVMModuleRef module, LibLLVMComdatIteratorCallback callback );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMComdatRef LibLLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, [MarshalUsing( typeof(AnsiStringMarshaller) )]string name, LLVMComdatSelectionKind kind );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMModuleComdatClear( LLVMModuleRef module );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalUsing( typeof( ErrorMessageMarshaller ) )]
        public static unsafe partial string LibLLVMComdatGetName( LLVMComdatRef comdatRef );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetFirstGlobalAlias( LLVMModuleRef M );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMValueRef LibLLVMModuleGetNextGlobalAlias( LLVMValueRef valueRef );
    }
}
