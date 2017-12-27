// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    internal class LLVMValueCacheRef
        : LlvmObjectRef
    {
        internal LLVMValueCacheRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        protected override bool ReleaseHandle( )
        {
            LLVMDisposeValueCache( handle );
            return true;
        }

        private LLVMValueCacheRef( )
            : base( true )
        {
        }

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeValueCache( IntPtr @C );
    }
}
