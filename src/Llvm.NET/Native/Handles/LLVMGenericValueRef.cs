// <copyright file="LLVMPassRegistryRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMGenericValueRef
        : LlvmObjectRef
    {
        internal LLVMGenericValueRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMDisposeGenericValue( handle );
            return true;
        }

        // ReSharper disable once UnusedMember.Local
        // used implicitly by interop marshaling
        private LLVMGenericValueRef( )
            : base( true )
        {
        }
    }
}
