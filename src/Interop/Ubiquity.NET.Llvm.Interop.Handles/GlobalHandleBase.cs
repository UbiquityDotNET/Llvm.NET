// <copyright file="SafeHandleNullIsInvalid.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Ubiquity.NET.Llvm.Interop.Handles
{
    /// <summary>Base class for global LLVM "ref" handle types that require cleanup.</summary>
    /// <remarks>
    /// This is a SafeHandle and therefore simplifies the use of LLVM handles in a managed runtime. This implements a common
    /// equality check and therefore depends on the fact that such "handles" are just pointers in the native layer. In particular,
    /// that it is valid to compare the pointers to different and unrelated types. This effectively establishes reference equality
    /// for the underlying native objects.
    /// <note type="note">
    /// It is worth noting that the marshaling will ALWAYS call the default constructor, then call SetHandle with the handle's
    /// value.</note>
    /// </remarks>
    [SecurityCritical]
    public abstract class GlobalHandleBase
        : SafeHandle
    {
        /// <inheritdoc/>
        public override bool IsInvalid
        {
            [SecurityCritical]
            get => handle == nint.Zero;
        }

        /// <summary>Throws an exception if this handle is invalid or in the closed state</summary>
        /// <param name="message">Exception error message [default: empty string]</param>
        /// <param name="memberName">Name of the member making the call [Default: provided by compiler]</param>
        /// <param name="sourceFilePath">Source file of the code making this call [Default: provided by compiler]</param>
        /// <param name="sourceLineNumber">Source line number of the code making this call [Default: provided by compiler]</param>
        /// <exception cref="LlvmException">This handle is invalid or closed</exception>
        public void ThrowIfInvalid(
            string message = "",
            [CallerMemberNameAttribute] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if(IsInvalid || IsClosed)
            {
                throw new LlvmException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber}; {message}" );
            }
        }

        /// <summary>Tests if this handle has the same value as <paramref name="other"/></summary>
        /// <param name="other">Other handle to compare with</param>
        /// <returns><see langword="true"/>if both handles refer to the same native instance</returns>
        /// <remarks>
        /// <note type = "important">
        /// It is important to note that this does NOT take into account the state of either handle.
        /// That is, if either is closed or invalid the wrapped handle value itself is what is tested
        /// so they are considered the same if the handle values are the same.
        /// </note>
        /// </remarks>
        public bool AreSame(GlobalHandleBase other)
        {
            return other is not null
                && DangerousGetHandle() == other.DangerousGetHandle();
        }

        /// <summary>Initializes a new instance of the <see cref="GlobalHandleBase"/> class with the specified value</summary>
        /// <param name="ownsHandle">true to reliably let System.Runtime.InteropServices.SafeHandle release the handle during
        /// the finalization phase; otherwise, false (not recommended).
        /// </param>
        protected GlobalHandleBase(bool ownsHandle)
            : base( nint.Zero, ownsHandle )
        {
        }

        protected GlobalHandleBase(nint handle, bool ownsHandle)
            : base(nint.Zero, ownsHandle)
        {
            SetHandle(handle);
        }

        protected GlobalHandleBase(nint handle)
            : this(ownsHandle: true)
        {
            SetHandle(handle);
        }
    }
}
