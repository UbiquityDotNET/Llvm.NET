// <copyright file="LLVMSharedObjectBufferRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
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
            NativeMethods.LLVMOrcDisposeSharedObjectBufferRef( handle );
            return true;
        }

        private LLVMSharedObjectBufferRef( )
            : base( true )
        {
        }
    }
}
