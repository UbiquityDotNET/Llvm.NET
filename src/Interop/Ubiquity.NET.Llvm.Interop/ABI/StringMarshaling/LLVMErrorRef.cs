﻿// -----------------------------------------------------------------------
// <copyright file="LLVMErrorRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Security;

namespace Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling
{
    /// <summary>Global LLVM object handle for an error</summary>
    /// <remarks>
    /// Lifetime control of an LLVM error message is tricky as there are calls that can "consume" the error which
    /// "invalidates" the handle making it an error to release it later. This class wraps those scenarios with a
    /// lazy approach. That is, no marshalling/copies of the underlying string are made until <see cref="ToString"/>
    /// is called.
    /// </remarks>
    /// <ImplementationNote>
    /// While this is a "handle" it is NOT a generated one as the behavior is VERY specialized. Thus, it lives in this
    /// library so it can leverage the <see cref="ErrorMessageString"/> for well defined release pattern of any error
    /// messages materialized. (<see cref="IDisposable"/>)
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

        /// <summary>Gets a value indicating whether this instance represents success</summary>
        public bool Success => handle == nint.Zero;

        /// <summary>Gets a value indicating whether this instance represents a failure</summary>
        public bool Failed => !Success;

        /// <inheritdoc/>
        public override string ToString()
        {
            return LazyMessage.Value?.ToString() ?? string.Empty;
        }

        public void ThrowIfFailed()
        {
            if (Failed)
            {
                throw new LlvmException(ToString());
            }
        }

        /// <summary>Gets the LLVM type ID for the error</summary>
        public LLVMErrorTypeId TypeId
        {
            get
            {
                return LLVMErrorTypeId.FromABI(LLVMGetErrorTypeId(handle));

                [DllImport( LibraryName )]
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

            [DllImport( LibraryName )]
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
                byte* bytes = ExecutionEncodingStringMarshaller.ConvertToUnmanaged(errMsg);
                try
                {
                    return new(LLVMCreateStringError(bytes));
                }
                finally
                {
                    ExecutionEncodingStringMarshaller.Free(bytes);
                }
            }

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static unsafe extern nint /*LLVMErrorRef*/ LLVMCreateStringError(byte* ErrMsg);
        }

        protected override void Dispose( bool disposing )
        {
            // if a message was previously realized, dispose of it now.
            if (disposing && LazyMessage.IsValueCreated)
            {
                LazyMessage.Value?.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc/>
        [ SecurityCritical]
        protected override bool ReleaseHandle()
        {
            LLVMConsumeError( handle );
            return true;

            [DllImport( LibraryName )]
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
            // Once the value is retrieved it cannot be retrieved again.
            // This marshals the string to an ErrorMessageString this one time
            // only. The ErrorMessageString instance owns the resulting native
            // string of the message and will dispose of it appropriately.
            ErrorMessageString retVal = new(LLVMGetErrorMessage( handle ));
            SetHandleAsInvalid();
            return retVal;

            // As this is a private local func, it must use DllImport AND
            // cannot allow ANY marshalling. That is done manually by caller.
            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern /*ErrorMessageString*/ nint LLVMGetErrorMessage(nint p);
        }

        // use Lazy to cache result of the underlying destructive get
        private readonly Lazy<ErrorMessageString?> LazyMessage;
    }
}
