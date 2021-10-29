// <copyright file="SafeHandleNullIsInvalid.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

using Ubiquity.ArgValidators;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Base class for global LLVM "ref" handle types that require cleanup.</summary>
    /// <remarks>
    /// This is a SafeHandle and therefore does not require that containing types implement IDisposable. This implements a common
    /// equality check and therefore depends on the fact that such "handles" are just pointers in the native layer, so that it is
    /// valid to compare the pointers to different and unrelated types. This effectively establishes reference equality for the
    /// underlying native objects.
    /// <note type="note">
    /// It is worth noting that the CLR marshaling will ALWAYS call the default constructor, then call SetHandle with the handle's
    /// value.</note>
    /// </remarks>
    [SecurityCritical]
    [SecurityPermission( SecurityAction.InheritanceDemand, UnmanagedCode = true )]
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
        public override int GetHashCode( ) => DangerousGetHandle( ).GetHashCode( );

        /// <inheritdoc/>
        public override bool Equals( object obj ) => Equals( obj as LlvmObjectRef );

        /// <inheritdoc/>
        public bool Equals( LlvmObjectRef? other ) => ( !( other is null ) ) && ( handle == other.handle );

        /// <summary>Compares two object handles</summary>
        /// <param name="lhs">Left side of comparison</param>
        /// <param name="rhs">Right side of comparison</param>
        /// <returns><see langword="true"/> if the handles are equal</returns>
        public static bool operator ==( LlvmObjectRef lhs, LlvmObjectRef rhs )
                => EqualityComparer<LlvmObjectRef>.Default.Equals( lhs, rhs );

        /// <summary>Compares two object handles for inequality</summary>
        /// <param name="lhs">Left side of comparison</param>
        /// <param name="rhs">Right side of comparison</param>
        /// <returns><see langword="true"/> if the handles are not equal</returns>
        public static bool operator !=( LlvmObjectRef lhs, LlvmObjectRef rhs ) => !( lhs == rhs );

        internal LlvmObjectRef ThrowIfInvalid(
            string message = "",
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0 )
        {
            return IsInvalid ? throw new LlvmException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber} {message} " ) : this;
        }

        /// <summary>Initializes a new instance of the <see cref="LlvmObjectRef"/> class with the specified value</summary>
        /// <param name="ownsHandle">true to reliably let System.Runtime.InteropServices.SafeHandle release the handle during
        /// the finalization phase; otherwise, false (not recommended).
        /// </param>
        [ReliabilityContract( Consistency.WillNotCorruptState, Cer.MayFail )]
        protected LlvmObjectRef( bool ownsHandle )
            : base( IntPtr.Zero, ownsHandle )
        {
        }
    }

    /// <summary>Extension class to allow fluent validation with derived types</summary>
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Considered part of the same type, must be static extension to support fluent use of derived types" )]
    public static class LlvmObjectRefExtensions
    {
        /// <summary>Fluent null handle validation</summary>
        /// <typeparam name="T">Type of handle to validate</typeparam>
        /// <param name="self">The handle to validate</param>
        /// <param name="message">Message to use for an exception if thrown</param>
        /// <param name="memberName">Name if the member calling this function (usually provided by compiler via <see cref="CallerMemberNameAttribute"/></param>
        /// <param name="sourceFilePath">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerFilePathAttribute"/></param>
        /// <param name="sourceLineNumber">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerLineNumberAttribute"/></param>
        /// <returns>This object for fluent style use</returns>
        public static T ThrowIfInvalid<T>(
            [ValidatedNotNull] this T self,
            string message = "",
            [CallerMemberNameAttribute] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0 )
            where T : LlvmObjectRef
        {
            return ( T )self.ValidateNotNull( nameof( self ) ).ThrowIfInvalid( message, memberName, sourceFilePath, sourceLineNumber );
        }
    }
}
