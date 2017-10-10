// <copyright file="LLVMTripleRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Security;

namespace Llvm.NET.Native
{
    // typedef struct LLVMOpaqueTriple* LLVMTripleRef;
    [SecurityCritical]
    internal class LLVMTripleRef
        : SafeHandleNullIsInvalid
    {
        internal LLVMTripleRef( )
            : base( true )
        {
        }

        internal LLVMTripleRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal LLVMTripleRef( IntPtr handle )
            : this( handle, false )
        {
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.DisposeTriple( handle );
            return true;
        }
    }
}
