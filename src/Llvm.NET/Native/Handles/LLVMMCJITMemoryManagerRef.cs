// <copyright file="LLVMPassRegistryRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMMCJITMemoryManagerRef
        : LlvmObjectRef
    {
        internal LLVMMCJITMemoryManagerRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMDisposeMCJITMemoryManager( handle );
            return true;
        }

        private LLVMMCJITMemoryManagerRef( )
            : base( true )
        {
        }
    }
}
