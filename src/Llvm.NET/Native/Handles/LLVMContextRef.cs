// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    internal class LLVMContextRef
        : LlvmObjectRef
    {
        internal LLVMContextRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        protected override bool ReleaseHandle( )
        {
            LLVMContextDispose( handle );
            return true;
        }

        private LLVMContextRef( )
            : base( true )
        {
        }

        [DllImport( NativeMethods.LibraryPath, EntryPoint = "LLVMContextDispose", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMContextDispose( IntPtr @C );
    }

    #pragma warning disable SA1402

    // Context alias
    internal class LLVMContextAlias
        : LLVMContextRef
    {
        private LLVMContextAlias()
            : base( IntPtr.Zero, false )
        {
        }
    }
}
