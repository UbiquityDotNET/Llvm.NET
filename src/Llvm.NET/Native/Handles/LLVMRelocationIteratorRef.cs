// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMRelocationIteratorRef
        : IEquatable<LLVMRelocationIteratorRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMRelocationIteratorRef r ) && r.Handle == Handle;

        public bool Equals( LLVMRelocationIteratorRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMRelocationIteratorRef lhs, LLVMRelocationIteratorRef rhs )
            => EqualityComparer<LLVMRelocationIteratorRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMRelocationIteratorRef lhs, LLVMRelocationIteratorRef rhs ) => !( lhs == rhs );

        internal LLVMRelocationIteratorRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
