// <copyright file="LLVMBuilderRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMBuilderRef
        : LlvmObjectRef
    {
        public LLVMBuilderRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMDisposeBuilder( handle );
            return true;
        }

        private LLVMBuilderRef( )
            : base( true )
        {
        }
    }
}
