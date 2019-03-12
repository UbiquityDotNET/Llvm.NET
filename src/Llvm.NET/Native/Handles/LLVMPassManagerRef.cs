// <copyright file="LLVMPassRegistryRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Security;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMPassManagerRef
        : LlvmObjectRef
    {
        internal LLVMPassManagerRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.LLVMDisposePassManager( handle );
            return true;
        }

        // ReSharper disable once UnusedMember.Local
        // used implicitly by interop marshaling
        private LLVMPassManagerRef( )
            : base( true )
        {
        }

        private static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMDisposePassManager", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposePassManager( IntPtr PM );
        }
    }
}
