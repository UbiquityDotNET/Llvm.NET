// -----------------------------------------------------------------------
// <copyright file="CStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling
{
    // Internal handle type for an LLVM Error message
    // See: LLVMErrorRef for more details of the oddities
    // of LLVM error message ownership
    internal class ErrorMessageString
        : CStringHandle
    {
        /// <summary>Default constructor is required for marshalling</summary>
        public ErrorMessageString( )
        {
        }

        public ErrorMessageString( nint hABI )
            : base( hABI )
        {
        }

        protected override bool ReleaseHandle( )
        {
            LLVMDisposeErrorMessage( handle );
            return true;

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeErrorMessage( nint p );
        }
    }
}
