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
            LLVMDisposeGenericValue( handle );
            return true;
        }

        // ReSharper disable once UnusedMember.Local
        // used implicitly by interop marshaling
        private LLVMGenericValueRef( )
            : base( true )
        {
        }

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeGenericValue", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeGenericValue( IntPtr GenVal );
    }
}
