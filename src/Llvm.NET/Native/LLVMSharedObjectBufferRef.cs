// <copyright file="LLVMSharedObjectBufferRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMSharedObjectBufferRef
        : SafeHandleNullIsInvalid
    {
        internal LLVMSharedObjectBufferRef( )
            : base( true )
        {
        }

        internal LLVMSharedObjectBufferRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal LLVMSharedObjectBufferRef( IntPtr handle )
            : this( handle, false )
        {
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMOrcDisposeSharedObjectBufferRef( handle );
            return true;
        }
    }
}
