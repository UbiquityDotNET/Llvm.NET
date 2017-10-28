// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMMDOperandRef
        : IEquatable<LLVMMDOperandRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMMDOperandRef r ) && r.Handle == Handle;

        public bool Equals( LLVMMDOperandRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMMDOperandRef lhs, LLVMMDOperandRef rhs )
            => EqualityComparer<LLVMMDOperandRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMMDOperandRef lhs, LLVMMDOperandRef rhs ) => !( lhs == rhs );

        internal LLVMMDOperandRef(IntPtr pointer)
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
