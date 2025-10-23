// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Error;

namespace Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling
{
    /// <summary>Global LLVM object handle for an error</summary>
    /// <remarks>
    /// Lifetime control of an LLVM error message is tricky as there are calls that can "consume" the error which
    /// "invalidates" the handle making it an exception to release it later. This class wraps those scenarios with a
    /// lazy approach. That is, no marshalling/copies of the underlying string are made until <see cref="ToString"/>
    /// is called. To this end, because it has mutable members (the lazy string) it is a class, NOT a struct to
    /// avoid all the aliasing problems that exist with a mutable struct and ownership.
    /// </remarks>
    /// <ImplementationNote>
    /// While this is a "handle" it is NOT a generated one as the behavior is VERY specialized. Thus, it lives in this
    /// library so it can leverage the <see cref="ErrorMessageString"/> for well defined release pattern of any error
    /// messages materialized. (<see cref="IDisposable"/>)
    /// </ImplementationNote>
    [NativeMarshalling(typeof(LLVMErrorRefMarshaller))]
    public sealed class LLVMErrorRef
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="LLVMErrorRef"/> class.</summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        public LLVMErrorRef( nint handle )
        {
            Handle = handle;

            // Error Ref by it's nature is single threaded, so no cross thread sync is needed
            LazyMessage = new( LazyGetMessage );
        }

        /// <summary>Gets a value indicating whether this instance represents success</summary>
        public bool Success => Handle == nint.Zero;

        /// <summary>Gets a value indicating whether this handle is a <see langword="null"/> value</summary>
        public bool IsNull => Handle == nint.Zero;

        /// <summary>Gets a value indicating whether this instance represents a failure</summary>
        public bool Failed => !Success;

        public bool IsString => LazyMessage.IsValueCreated || TypeId == LLVMGetStringErrorTypeId();

        /// <summary>Gets a string representation of the error message</summary>
        /// <returns>String representation of the error message</returns>
        public override string ToString( )
        {
            if(IsNull)
            {
                return string.Empty;
            }

            if(IsString)
            {
                ErrorMessageString msgHandle = LazyMessage.Value;
                return msgHandle.IsNull ? string.Empty : msgHandle.ToManaged();
            }
            else
            {
                // in the event of a non-string error, just indicate the type ID as there is
                // no LLVM-C means of accessing the data. It is unclear where this can occur
                // though the overwhelming majority of cases is for a string.
                return $"TypeID: {TypeId.DangerousGetHandle()}";
            }
        }

        /// <summary>Gets the type ID for this error message</summary>
        /// <remarks>
        /// The support for non-string types is vague at best. LLVM-C API
        /// does not support
        /// </remarks>
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Nested conditional is almost never 'simplified'" )]
        public LLVMErrorTypeId TypeId
        {
            get
            {
                if(IsNull)
                {
                    return default;
                }

                return LazyMessage.IsValueCreated
                     ? LLVMGetStringErrorTypeId() // string retrieved already so the "type" is known.
                     : LLVMErrorTypeId.FromABI(LLVMGetErrorTypeId(DangerousGetHandle()));

                [DllImport( LibraryName )]
                [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
                [SuppressMessage( "Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "Signature is P/Invoke ready" )]
                static extern /*LLVMErrorTypeId*/nint LLVMGetErrorTypeId(/*LLVMErrorRef*/ nint Err );
            }
        }

        public void ThrowIfFailed( )
        {
            if(Failed)
            {
                throw new LlvmException( ToString() );
            }
        }

        /// <summary>Reports a failure error if this is a failure condition</summary>
        public void CantFail( )
        {
            if(Handle != nint.Zero)
            {
                LLVMCantFail( Handle );
            }

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMCantFail(/*LLVMErrorRef*/ nint Err );
        }

        /// <summary>Create a new <see cref="LLVMErrorRef"/> (StringError) from a managed string</summary>
        /// <param name="errMsg">Message for the error </param>
        /// <returns>new instance</returns>
        /// <remarks>
        /// This is a static factory as there is no default construction provided
        /// in the LLVM-C API.
        /// </remarks>
        public static LLVMErrorRef Create( LazyEncodedString errMsg )
        {
            return LLVMCreateStringError( errMsg );
        }

        /// <summary>Create a new <see cref="LLVMErrorRef"/> as a <see cref="nint"/> from a <see cref="LazyEncodedString"/></summary>
        /// <param name="errMsg">Message for the error </param>
        /// <returns>new instance</returns>
        /// <remarks>
        /// <para>This is a static factory as there is no default construction provided
        /// in the LLVM-C API.</para>
        /// <para>This form of the factory is normally used directly only for out/return reverse P/Invoke scenarios.
        /// In all other cases a fully wrapped handle (<see cref="LLVMErrorRef"/>) is used via <see cref="Create(LazyEncodedString)"/>.
        /// </para>
        /// </remarks>
        public static nint CreateForNativeOut( LazyEncodedString errMsg )
        {
            unsafe
            {
                // handle the marshalling to convert it directly.
                // Theoretically the source generator supports reverse P/Invoke marshalling,
                // but there is NO documentation or obvious options to "invoke" such a thing.
                // So, until that is found this is done "manually" here.
                using var mem = errMsg.Pin();
                return LLVMCreateStringError( (byte*)mem.Pointer );
            }

            [DllImport( LibraryName, EntryPoint = "LLVMCreateStringError" )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static unsafe extern nint /*LLVMErrorRef*/ LLVMCreateStringError( byte* ErrMsg );
        }

        /// <summary>Create a new <see cref="LLVMErrorRef"/> as <see cref="nint"/> from <see cref="Exception.Message"/></summary>
        /// <param name="ex">Exceotion to get the error message from</param>
        /// <inheritdoc cref="CreateForNativeOut(LazyEncodedString)"/>
        public static nint CreateForNativeOut( Exception ex)
        {
            return CreateForNativeOut(ex.Message);
        }

        public void Dispose()
        {
            // if a message was previously realized, dispose of it now.
            // Otherwise, consume it
            if(LazyMessage.IsValueCreated)
            {
                LazyMessage.Value.Dispose();
            }
            else if(Handle != nint.Zero)
            {
                LLVMConsumeError( Handle );
            }

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMConsumeError( nint p );
        }

        public nint DangerousGetHandle( )
        {
            return Handle;
        }

        public static LLVMErrorRef FromABI( nint abiValue )
        {
            return new(abiValue);
        }

        private ErrorMessageString LazyGetMessage( )
        {
            if(IsNull)
            {
                return default;
            }

            // LLVMGetErrorMessage is explicitly defined as NOT idempotent.
            // Once the value is retrieved it cannot be retrieved again.
            // This marshals the string to an ErrorMessageString this one time
            // only. The ErrorMessageString instance owns the resulting native
            // string of the message and will dispose of it appropriately.
            ErrorMessageString retVal = new(LLVMGetErrorMessage( Handle ));
            return retVal;

            // As this is a private local func, it must use DllImport AND
            // cannot allow ANY marshalling. That is done manually by caller.
            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern /*ErrorMessageString*/ nint LLVMGetErrorMessage( nint p );
        }

        private readonly nint Handle;

        // use Lazy to cache result of the underlying destructive get
        private readonly Lazy<ErrorMessageString> LazyMessage;
    }
}
