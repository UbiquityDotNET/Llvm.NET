// <copyright file="SafeHandleNullIsInvalid.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace Ubiquity.NET.Llvm.Interop
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
    public abstract class LlvmObjectRef
        : SafeHandle
        , IEquatable<LlvmObjectRef>
    {
        /// <inheritdoc/>
        public override bool IsInvalid
        {
            [SecurityCritical]
            get => handle == IntPtr.Zero;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => DangerousGetHandle().GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals( obj as LlvmObjectRef );

        /// <inheritdoc/>
        public bool Equals(LlvmObjectRef? other) => (other is not null) && (handle == other.handle);

        /// <summary>Compares two object handles</summary>
        /// <param name="lhs">Left side of comparison</param>
        /// <param name="rhs">Right side of comparison</param>
        /// <returns><see langword="true"/> if the handles are equal</returns>
        public static bool operator ==(LlvmObjectRef lhs, LlvmObjectRef rhs)
            => EqualityComparer<LlvmObjectRef>.Default.Equals( lhs, rhs );

        /// <summary>Compares two object handles for inequality</summary>
        /// <param name="lhs">Left side of comparison</param>
        /// <param name="rhs">Right side of comparison</param>
        /// <returns><see langword="true"/> if the handles are not equal</returns>
        public static bool operator !=(LlvmObjectRef lhs, LlvmObjectRef rhs) => !(lhs == rhs);

        public void ThrowIfInvalid(
            string message = "",
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if(IsInvalid)
            {
                throw new LlvmException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber}; {message}" );
            }
        }

        /// <summary>Initializes a new instance of the <see cref="LlvmObjectRef"/> class with the specified value</summary>
        /// <param name="ownsHandle">true to reliably let System.Runtime.InteropServices.SafeHandle release the handle during
        /// the finalization phase; otherwise, false (not recommended).
        /// </param>
        [SuppressMessage( "Style", "IDE0290:Use primary constructor", Justification = "Visibility of constructor is not expressible with a primary constructor" )]
        protected LlvmObjectRef(bool ownsHandle)
            : base( nint.Zero, ownsHandle )
        {
        }

        protected LlvmObjectRef(nint handle, bool ownsHandle)
            : base(nint.Zero, ownsHandle)
        {
            SetHandle(handle);
        }

        protected LlvmObjectRef(nint handle)
            : this(ownsHandle: true)
        {
            SetHandle(handle);
        }
    }
}
