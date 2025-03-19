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
        /// <summary>Gets a value indicating whether this instance represents success</summary>
        public bool Success
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, typeof(ErrorInfo));

                return Handle.Success;
            }
        }

        /// <summary>Gets a value indicating whether this instance represents a failure</summary>
        public bool Failed => !Success;

        /// <summary>Gets a value indicating whether this instance is disposed</summary>
        public bool IsDisposed => Handle is not null && Handle.IsClosed;

        /// <inheritdoc/>
        public override string ToString()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, typeof(ErrorInfo));

            return Handle.ToString();
        }

        /// <summary>Throws an exception if this instance is a failure result (<see cref="Failed"/> is <see langword="true"/>)</summary>
        /// <exception cref="InternalCodeGeneratorException"><see cref="Failed"/> is <see langword="true"/></exception>
        /// <remarks>
        /// The <see cref="Exception.Message"/> is set to the text of this error result.
        /// </remarks>
        public void ThrowIfFailed()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, typeof(ErrorInfo));

            if (Failed)
            {
                throw new InternalCodeGeneratorException(ToString());
            }
        }

        /// <summary>Releases the underlying LLVM handle</summary>
        public void Dispose()
        {
            Handle.Dispose();
        }

        /// <summary>Factory function to create a new <see cref="ErrorInfo"/> from a string</summary>
        /// <param name="msg">message for the error</param>
        /// <returns>Newly constructed <see cref="ErrorInfo"/> with the provided message</returns>
        public static ErrorInfo Create(string msg)
        {
            return ErrorInfo.Create(msg);
        }

        /// <summary>Factory function to create a new <see cref="ErrorInfo"/> from an exception</summary>
        /// <param name="ex">Exception to create an instance from</param>
        /// <returns>Newly constructed <see cref="ErrorInfo"/> with the message from <paramref name="ex"/></returns>
        /// <remarks>
        /// <note type="important">
        /// It is important to note that this will convert a <see langword="null"/> for <paramref name="ex"/> into
        /// a failed result with an empty string. This is to prevent exception within a catch handler. This
        /// condition is asserted in a debug build so that any attempts to provide a null value are caught and
        /// fixed.
        /// </note>
        /// </remarks>
        public static ErrorInfo Create(Exception ex)
        {
            Debug.Assert(ex is not null, "ERROR: Must not provide NULL - debug assert prevents exception in catch handler. FIX THE CALLER - it's broken!");
            return Create(ex?.Message ?? string.Empty);
        }

        /// <summary>This provides `move` semantics when transferring ownership of the resources represented by the handle to native code</summary>
        /// <returns>The underlying native handle that will NOT receive any additional clean up or release</returns>
        /// <remarks>
        /// <note type="important">It is important to note that this will release all the safety guarantees of cleanup for a
        /// <see cref="SafeHandle"/>! This is normally used directly as the return value of a callback. It should **NOT** be
        /// used for `in` parameters. It is possible that the ownership is not fully transferred and some error results leaving
        /// the resource dangling/leaked. Instead, pass it using normal handle marshalling, then after the native call returns it
        /// is safe to call <see cref="SafeHandle.SetHandleAsInvalid"/> to mark it as unowned.
        /// </note>
        /// </remarks>
        internal nint MoveToNative()
        {
            return Handle?.MoveToNative() ?? 0;
        }

        internal ErrorInfo(LLVMErrorRef h)
        {
            ArgumentNullException.ThrowIfNull(h);
            Handle = h.Move();
        }

        /// <summary>Initializes a new instance of the <see cref="ErrorInfo"/> struct from a native handle</summary>
        /// <param name="h">native handle to wrap</param>
        /// <remarks>
        /// This is generally used in unmanaged callbacks to simplify creation of the projection from the raw handle.
        /// </remarks>
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is 'moved' to this type; dispose not needed" )]
        internal ErrorInfo(nint h)
            : this(new LLVMErrorRef(h))
        {
        }

        private readonly LLVMErrorRef Handle;
    }
}
