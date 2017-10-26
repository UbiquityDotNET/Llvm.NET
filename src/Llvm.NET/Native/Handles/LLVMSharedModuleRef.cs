// <copyright file="LLVMSharedModuleRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMSharedModuleRef
        : SafeHandleNullIsInvalid
    {
        internal LLVMSharedModuleRef( )
            : base( true )
        {
        }

        internal LLVMSharedModuleRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal LLVMSharedModuleRef( IntPtr handle )
            : this( handle, false )
        {
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMOrcDisposeSharedModuleRef( handle );
            return true;
        }
    }
}
