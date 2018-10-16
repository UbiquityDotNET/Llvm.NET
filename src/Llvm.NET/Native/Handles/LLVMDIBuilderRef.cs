// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Security;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMDIBuilderRef
        : LlvmObjectRef
    {
        public LLVMDIBuilderRef( IntPtr handle, bool owner)
            : base( owner )
        {
            SetHandle( handle );
        }

        protected override bool ReleaseHandle( )
        {
            LLVMDIBuilderDestroy( handle );
            return true;
        }

        // ReSharper disable once UnusedMember.Local
        // used implicitly by interop marshaling
        private LLVMDIBuilderRef()
            : base( true )
        {
        }

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDIBuilderDestroy( IntPtr d );
    }
}
