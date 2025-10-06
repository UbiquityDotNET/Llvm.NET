// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling
{
    // Internal handle type for an LLVM Error message
    // See: LLVMErrorRef for more details of the oddities
    // of LLVM error message ownership
    internal readonly record struct ErrorMessageString
        : IWrappedHandle<ErrorMessageString>
        , IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="ErrorMessageString"/> struct.</summary>
        /// <remarks>Default constructor is required for marshalling</remarks>
        public ErrorMessageString( )
        {
        }

        public ErrorMessageString( nint hABI )
        {
            Handle = hABI;
        }

        public bool IsNull => Handle == nint.Zero;

        public void Dispose( )
        {
            LLVMDisposeErrorMessage( Handle );

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeErrorMessage( nint p );
        }

        public LazyEncodedString ToManaged()
        {
            unsafe
            {
                return LazyEncodedString.FromUnmanaged((byte*)Handle.ToPointer());
            }
        }

        public readonly nint DangerousGetHandle( )
        {
            return Handle;
        }

        public static ErrorMessageString FromABI( nint abiValue )
        {
            return new(abiValue);
        }

        private readonly nint Handle; // char*
    }
}
