// <copyright file="LLVMSharedObjectBufferRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMSharedObjectBufferRef
        : LlvmObjectRef
    {
        internal LLVMSharedObjectBufferRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            LLVMOrcDisposeSharedObjectBufferRef( handle );
            return true;
        }

        private LLVMSharedObjectBufferRef( )
            : base( true )
        {
        }

        [DllImport( NativeMethods.LibraryPath, EntryPoint = "LLVMOrcDisposeSharedObjectBufferRef", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMOrcDisposeSharedObjectBufferRef( IntPtr SharedObjBuffer );
    }
}
