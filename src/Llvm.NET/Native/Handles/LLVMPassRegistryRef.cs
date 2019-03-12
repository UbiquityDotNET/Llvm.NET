// <copyright file="LLVMPassRegistryRefRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Security;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMPassRegistryRef
        : LlvmObjectRef
    {
        internal LLVMPassRegistryRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            if( !IsInvalid && !IsClosed )
            {
                LLVMPassRegistryDispose( handle );
                SetHandleAsInvalid( );
            }

            return true;
        }

        // ReSharper disable once UnusedMember.Local
        // used implicitly by interop marshaling
        private LLVMPassRegistryRef( )
            : base( true )
        {
        }

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMPassRegistryDispose( IntPtr hPassRegistry );
    }
}
