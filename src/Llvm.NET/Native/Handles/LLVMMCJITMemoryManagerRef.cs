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
    internal class LLVMMCJITMemoryManagerRef
        : LlvmObjectRef
    {
        internal LLVMMCJITMemoryManagerRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            LLVMDisposeMCJITMemoryManager( handle );
            return true;
        }

        // ReSharper disable once UnusedMember.Local
        // used implicitly by interop marshaling
        private LLVMMCJITMemoryManagerRef( )
            : base( true )
        {
        }

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeMCJITMemoryManager", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeMCJITMemoryManager( IntPtr MM );
    }
}
