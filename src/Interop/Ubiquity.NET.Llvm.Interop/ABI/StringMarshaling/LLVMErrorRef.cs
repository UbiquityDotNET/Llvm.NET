// -----------------------------------------------------------------------
// <copyright file="LLVMErrorRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Global LLVM object handle for an error</summary>
    /// <remarks>
    /// Lifetime control of an LLVM error message is tricky as there are calls that can "consume" the error which
    /// "invalidates" the handle making it an error to release it later. This class wraps those scenarios with a
    /// lazy approach. That is, no marshalling/copies of the underlying string are made until <see cref="ToString"/>
    /// is called.
    /// </remarks>
    /// <ImplementationNote>
    /// While this is a "handle" it is NOT a generated one as the behavior is VERY specialized. So it lives in this
    /// library in the "Handles" namespace so it can leverage the <see cref="ErrorMessageString"/> for release of any
    /// error messages materialized.
    /// </ImplementationNote>
    [SecurityCritical]
    public class LLVMErrorRef
        : GlobalHandleBase
    {
        /// <summary>Initializes a new instance of the <see cref="LLVMErrorRef"/> class with default values for marshalling</summary>
        public LLVMErrorRef()
            : base( true )
        {
            LazyMessage = new( LazyGetMessage );
        }

        /// <summary>Initializes a new instance of the <see cref="LLVMErrorRef"/> class.</summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        public LLVMErrorRef(nint handle)
            : base( true )
        {
            SetHandle( handle );
            LazyMessage = new( LazyGetMessage );
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return LazyMessage.Value?.ToString() ?? string.Empty;
        }

        /// <summary>Gets the LLVM type ID for the error</summary>
        public LLVMErrorTypeId TypeId
        {
            get
            {
                return LLVMErrorTypeId.FromABI(LLVMGetErrorTypeId(handle));

                [DllImport( NativeMethods.LibraryPath )]
                [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
                static extern /*LLVMErrorTypeId*/nint LLVMGetErrorTypeId(/*LLVMErrorRef*/ nint Err);
            }
        }

        /// <summary>Reports a failure error if this is a failure condition</summary>
        public void CantFail()
        {
            if (!IsClosed && !IsInvalid)
            {
                LLVMCantFail(handle);
            }

            [DllImport( NativeMethods.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMCantFail(/*LLVMErrorRef*/ nint Err);
        }

        /// <summary>Create a new <see cref="LLVMErrorRef"/> (StringError) from a managed string</summary>
        /// <param name="errMsg">Message for the error </param>
        /// <returns>new instance</returns>
        /// <remarks>
        /// This is a static factory as there is no default construction provided
        /// in the LLVM-C API.
        /// </remarks>
        public static LLVMErrorRef Create(string errMsg)
        {
            unsafe
            {
                byte* bytes = AnsiStringMarshaller.ConvertToUnmanaged(errMsg);
                try
                {
                    return new(LLVMCreateStringError(bytes));
                }
                finally
                {
                    AnsiStringMarshaller.Free(bytes);
                }
            }

            [DllImport( NativeMethods.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static unsafe extern nint /*LLVMErrorRef*/ LLVMCreateStringError(byte* ErrMsg);
        }

        /// <inheritdoc/>
        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            LLVMConsumeError( handle );
            return true;

            [DllImport( NativeMethods.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMConsumeError(nint p);
        }

        private ErrorMessageString? LazyGetMessage()
        {
            if(IsClosed || IsInvalid)
            {
                return null;
            }

            // LLVMGetErrorMessage is explicitly defined as NOT idempotent.
            // So, once the value is retrieved it cannot be retrieved again.
            // This marshals the string to an ErrorMessageString this one time
            // only. The ErrorMessageString owns the resulting string of the
            // message.
            ErrorMessageString retVal = new(LLVMGetErrorMessage( handle ));
            SetHandleAsInvalid();
            return retVal;

            // As this is a private local func, it must use DllImport AND
            // cannot allow ANY marshalling. That is done manually by caller.
            [DllImport( NativeMethods.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern /*ErrorMessageString*/ nint LLVMGetErrorMessage(nint p);
        }

        // use Lazy to cache result of the underlying destructive get
        private readonly Lazy<ErrorMessageString?> LazyMessage;
    }
}
