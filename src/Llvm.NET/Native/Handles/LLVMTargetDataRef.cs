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
    internal class LLVMTargetDataRef
        : LlvmObjectRef
    {
        public LLVMTargetDataRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            LLVMDisposeTargetData( handle );
            return true;
        }

        private LLVMTargetDataRef( )
            : base( true )
        {
        }

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeTargetData( IntPtr TargetData );
    }

#pragma warning disable SA1402

    // TargetData alias
    internal class LLVMTargetDataAlias
        : LLVMTargetDataRef
    {
        private LLVMTargetDataAlias( )
            : base( IntPtr.Zero, false )
        {
        }
    }
}
