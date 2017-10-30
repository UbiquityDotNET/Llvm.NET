// <copyright file="LLVMSharedModuleRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMSharedModuleRef
        : LlvmObjectRef
    {
        internal LLVMSharedModuleRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMOrcDisposeSharedModuleRef( handle );
            return true;
        }

        private LLVMSharedModuleRef( )
            : base( true )
        {
        }
    }
}
