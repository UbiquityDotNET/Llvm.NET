// -----------------------------------------------------------------------
// <copyright file="LLVMErrorRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Global LLVM object handle for an error</summary>
    /// <remarks>
    /// Lifetime control of an LLVM error message is tricky as there are
    /// calls that can "consume" the error which "invalidates" the handle
    /// making it an error to release it later. This class wraps those
    /// scenarios.
    /// </remarks>
    [SecurityCritical]
    [SuppressMessage( "Design", "CA1060:Move pinvokes to native methods class", Justification = "Not for use by any other callers" )]
    public partial class LLVMErrorRef
        : LlvmObjectRef
    {
        /// <summary>Initializes a new instance of the <see cref="LLVMErrorRef"/> class with default values for marshalling</summary>
        public LLVMErrorRef()
            : base( true )
        {
            LazyMessage = new Lazy<string>( InternalGetMessage );
        }

        /// <summary>Initializes a new instance of the <see cref="LLVMErrorRef"/> class.</summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        /// <param name="owner">Value to indicate whether the handle is owned or not</param>
        public LLVMErrorRef(nint handle, bool owner)
            : base( owner )
        {
            SetHandle( handle );
            LazyMessage = new Lazy<string>( InternalGetMessage );
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return LazyMessage.Value;
        }

        /// <inheritdoc/>
        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            if(handle != nint.Zero)
            {
                LLVMConsumeError( handle );
            }

            return true;
        }

        private string InternalGetMessage()
        {
            if(IsInvalid)
            {
                return string.Empty;
            }

            // LLVMGetErrorMessage is explicitly defined as NOT idempotent.
            // so once the value is retrieved the pointer is no longer valid
            string retVal = LLVMGetErrorMessage( handle );
            SetHandleAsInvalid();
            return retVal;
        }

        // use Lazy to cache result of the underlying destructive get
        private readonly Lazy<string> LazyMessage;

        [LibraryImport( NativeMethods.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMConsumeError(nint p);

        [LibraryImport( NativeMethods.LibraryPath, StringMarshallingCustomType = typeof( ErrorMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial string LLVMGetErrorMessage(nint p);
    }
}
