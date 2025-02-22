// -----------------------------------------------------------------------
// <copyright file="LlvmStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    // Internal handle type for an LLVM Error message
    // See: LLVMErrorRef for more details of the oddities
    // of LLVM error message ownership
    internal class ErrorMessageString
        : LlvmStringHandle
    {
        public ErrorMessageString(nint hABI)
            : base( hABI )
        {
        }

        protected override bool ReleaseHandle()
        {
            LLVMDisposeErrorMessage( handle );
            return true;

            [DllImport( Names.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeErrorMessage(nint p);
        }
    }
}
