// <copyright file="LLVMPassRegistryRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMPassManagerRef
        : SafeHandleNullIsInvalid
    {
        internal LLVMPassManagerRef( )
            : base( true )
        {
        }

        internal LLVMPassManagerRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal LLVMPassManagerRef( IntPtr handle )
            : this( handle, false )
        {
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMDisposePassManager( handle );
            return true;
        }
    }
}
