﻿// <copyright file="SafeHandleNullIsInvalid.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Llvm.NET.Interop
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
        public override bool IsInvalid
        {
            [SecurityCritical]
            get => handle == IntPtr.Zero;
        }

        public override int GetHashCode( ) => DangerousGetHandle( ).GetHashCode( );

        public override bool Equals( object obj ) => Equals( obj as LlvmObjectRef );

        public bool Equals( LlvmObjectRef other ) => ( !( other is null ) ) && ( handle == other.handle );

        public static bool operator ==( LlvmObjectRef lhs, LlvmObjectRef rhs )
            => EqualityComparer<LlvmObjectRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LlvmObjectRef lhs, LlvmObjectRef rhs ) => !( lhs == rhs );

        [ReliabilityContract( Consistency.WillNotCorruptState, Cer.MayFail )]
        protected LlvmObjectRef( bool ownsHandle )
            : base( IntPtr.Zero, ownsHandle )
        {
        }
    }
}
