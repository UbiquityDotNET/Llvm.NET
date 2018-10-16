// <copyright file="LLVMSharedModuleRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
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
            LLVMOrcDisposeSharedModuleRef( handle );
            return true;
        }

        // ReSharper disable once UnusedMember.Local
        // used implicitly by interop marshaling
        private LLVMSharedModuleRef( )
            : base( true )
        {
        }

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMOrcDisposeSharedModuleRef( IntPtr SharedMod );
    }
}
