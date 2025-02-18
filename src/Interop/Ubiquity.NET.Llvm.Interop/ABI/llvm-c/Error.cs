// -----------------------------------------------------------------------
// <copyright file="Error.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMErrorTypeId LLVMGetErrorTypeId( LLVMErrorRef Err );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LLVMCantFail( LLVMErrorRef Err );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMErrorTypeId LLVMGetStringErrorTypeId();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMErrorRef LLVMCreateStringError( [MarshalUsing( typeof(AnsiStringMarshaller) )]string ErrMsg );
    }
}
