// -----------------------------------------------------------------------
// <copyright file="ErrorInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    /// <summary>Represents the success or failure of an operation with "try" semantics</summary>
    /// <remarks>
    /// In addition to the <see cref="Success"/> and <see cref="Failed"/> states this also tracks
    /// any error messages in the event of a failure.
    /// </remarks>
    public readonly ref struct ErrorInfo
    {
        /// <summary>Initializes a new instance of the <see cref="ErrorInfo"/> struct</summary>
        /// <param name="msg">Message for the error</param>
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "Ownership is transferred to this instances. (Move semantics)" )]
        public ErrorInfo( LazyEncodedString msg )
            : this( LLVMErrorRef.Create( msg ) )
        {
        }

        /// <summary>Gets a value indicating whether this instance represents success</summary>
        public bool Success
        {
            get
            {
                ObjectDisposedException.ThrowIf( IsDisposed, typeof( ErrorInfo ) );
                return Handle.Success;
            }
        }

        /// <summary>Gets a value indicating whether this instance represents a failure</summary>
        public bool Failed => !Success;

        /// <summary>Gets a value indicating whether this instance is disposed</summary>
        public bool IsDisposed => Handle is not null && Handle.IsClosed;

        /// <summary>Gets a string representation of the error message</summary>
        /// <returns>String representation of the error message or an empty string if none</returns>
        public override string ToString( )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, typeof( ErrorInfo ) );
            return Handle.ToString();
        }

        /// <summary>Throws an exception if this instance is a failure result (<see cref="Failed"/> is <see langword="true"/>)</summary>
        /// <exception cref="InternalCodeGeneratorException"><see cref="Failed"/> is <see langword="true"/></exception>
        /// <remarks>
        /// The <see cref="Exception.Message"/> is set to the text of this error result.
        /// </remarks>
        public void ThrowIfFailed( )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, typeof( ErrorInfo ) );

            if(Failed)
            {
                throw new InternalCodeGeneratorException( ToString() );
            }
        }

        /// <summary>Releases the underlying LLVM handle</summary>
        public void Dispose( )
        {
            Handle.Dispose();
        }

        /// <summary>This provides `move` semantics when transferring ownership of the resources represented by the handle to native code</summary>
        /// <returns>The underlying native handle that will NOT receive any additional clean up or release</returns>
        /// <remarks>
        /// <note type="important">It is important to note that this will release all the safety guarantees of cleanup for a
        /// <see cref="SafeHandle"/>! This is normally used directly as the return value of a callback. It should **NOT** be
        /// used for `in` parameters. In that scenario, it is possible that the ownership is not fully transferred and some
        /// error results leaving the resource dangling/leaked. Instead, pass it using normal handle marshalling, then after
        /// the native call returns it is safe to call <see cref="SafeHandle.SetHandleAsInvalid"/> to mark it as unowned.
        /// </note>
        /// </remarks>
        internal nint MoveToNative( )
        {
            return Handle?.MoveToNative() ?? 0;
        }

        internal ErrorInfo( LLVMErrorRef h )
        {
            ArgumentNullException.ThrowIfNull( h );
            Handle = h.Move();
        }

        /// <summary>Initializes a new instance of the <see cref="ErrorInfo"/> struct from a native handle</summary>
        /// <param name="h">native handle to wrap</param>
        /// <remarks>
        /// This is generally used in unmanaged callbacks to simplify creation of the projection from the raw handle.
        /// </remarks>
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is 'moved' to this type; dispose not needed" )]
        internal ErrorInfo( nint h )
            : this( new LLVMErrorRef( h ) )
        {
        }

        private readonly LLVMErrorRef Handle;
    }
}
