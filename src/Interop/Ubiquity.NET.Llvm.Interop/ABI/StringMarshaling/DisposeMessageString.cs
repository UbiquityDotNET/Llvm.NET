// -----------------------------------------------------------------------
// <copyright file="LlvmStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Manages Common LLVM strings that use the `LLVMDisposeMessage` API to release the resources for the string</summary>
    public class DisposeMessageString
        : LlvmStringHandle
    {
        /// <summary>Default constructor is required for marshalling</summary>
        public DisposeMessageString()
        {
        }

        protected override bool ReleaseHandle()
        {
            LLVMDisposeMessage( handle );
            return true;

            [DllImport( NativeMethods.LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeMessage(nint p);
        }
    }
}
