// -----------------------------------------------------------------------
// <copyright file="ErrorInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Represents the success or failure of an operation with "try" semantics</summary>
    /// <remarks>
    /// In addition to the <see cref="Success"/> and <see cref="Failed"/> states this also tracks
    /// any error messages in the event of a failure.
    /// </remarks>
    public readonly ref struct ErrorInfo
    {
        /// <summary>Initializes a new instance of the <see cref="ErrorInfo"/> struct.</summary>
        /// <param name="safeHandle">interop handle to initialize from</param>
        /// <exception cref="ArgumentException">The handle isn't of the correct type</exception>
        /// <remarks>
        /// <note type="important">
        /// Use of This API outside of the ORC library is NOT supported. Do not attempt to use it
        /// in application code.
        /// </note>
        /// </remarks>
        public ErrorInfo(SafeHandle safeHandle)
        {
            if( safeHandle is not LLVMErrorRef h)
            {
                throw new ArgumentException("Incorrect handle type provided");
            }

            Handle = h;
        }

        /// <summary>Gets a value indicating whether this instance represents success</summary>
        public bool Success => Handle.Success;

        /// <summary>Gets a value indicating whether this instance represents a failure</summary>
        public bool Failed => !Success;

        /// <inheritdoc/>
        public override string ToString()=> Handle.ToString();

        /// <summary>Throws an exception if this instance is a failure result (<see cref="Failed"/> is <see langword="true"/>)</summary>
        /// <exception cref="InternalCodeGeneratorException"><see cref="Failed"/> is <see langword="true"/></exception>
        /// <remarks>
        /// The <see cref="Exception.Message"/> is set to the text of this error result.
        /// </remarks>
        public void ThrowIfFailed()
        {
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

        internal ErrorInfo(LLVMErrorRef h)
        {
            ArgumentNullException.ThrowIfNull(h);
            Handle = h;
        }

        /// <summary>Initializes a new instance of the <see cref="ErrorInfo"/> struct from a native handle</summary>
        /// <param name="h">native handle to wrap</param>
        /// <remarks>
        /// This is generally used in unmanaged callbacks to simplify creation of the projection from the raw handle.
        /// </remarks>
        internal ErrorInfo(nint h)
            : this(new LLVMErrorRef(h))
        {
        }

        private readonly LLVMErrorRef Handle;
    }
}
