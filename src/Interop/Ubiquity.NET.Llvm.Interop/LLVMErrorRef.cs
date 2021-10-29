// -----------------------------------------------------------------------
// <copyright file="LLVMErrorRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Global LLVM object handle</summary>
    [SecurityCritical]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Design", "CA1060:Move pinvokes to native methods class", Justification = "private to this class and MUST never be used anywhere else" )]
    public class LLVMErrorRef
        : LlvmObjectRef
    {
        /// <summary>Initializes a new instance of the <see cref="LLVMErrorRef"/> class.</summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        /// <param name="owner">Value to indicate whether the handle is owned or not</param>
        public LLVMErrorRef( IntPtr handle, bool owner )
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
        protected override bool ReleaseHandle( )
        {
            // ensure handle appears invalid from this point forward
            var prevHandle = Interlocked.Exchange( ref handle, IntPtr.Zero );
            SetHandleAsInvalid( );

            if( prevHandle != IntPtr.Zero )
            {
                LLVMConsumeError( handle );
            }

            return true;
        }

        // during marshaling the runtime always calls the default constructor and calls SetHandle()
        private LLVMErrorRef( )
            : base( true )
        {
            LazyMessage = new Lazy<string>( InternalGetMessage );
        }

        private string InternalGetMessage( )
        {
            if( IsInvalid )
            {
                return string.Empty;
            }

            string retVal = LLVMGetErrorMessage( handle );
            SetHandle( IntPtr.Zero );
            SetHandleAsInvalid( );
            return retVal;
        }

        // use Lazy to cache result of the underlying destructive get
        private readonly Lazy<string> LazyMessage;

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMConsumeError( IntPtr p );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( ErrorMessageMarshaler ) )]
        private static extern string LLVMGetErrorMessage( IntPtr p );
    }
}
