// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    internal class LLVMTargetMachineRef
        : LlvmObjectRef
    {
        internal LLVMTargetMachineRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        protected override bool ReleaseHandle( )
        {
            LLVMDisposeTargetMachine( handle );
            return true;
        }

        private LLVMTargetMachineRef( )
            : base( true )
        {
        }

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeTargetMachine( IntPtr targetMachine );
    }

#pragma warning disable SA1402

    internal class LLVMTargetMachineAlias
        : LLVMTargetMachineRef
    {
        private LLVMTargetMachineAlias( )
            : base( IntPtr.Zero, false )
        {
        }
    }
}
