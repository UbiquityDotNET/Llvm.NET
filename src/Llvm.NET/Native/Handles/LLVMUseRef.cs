// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMUseRef
        : IEquatable<LLVMUseRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMUseRef r ) && r.Handle == Handle;

        public bool Equals( LLVMUseRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMUseRef lhs, LLVMUseRef rhs )
            => EqualityComparer<LLVMUseRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMUseRef lhs, LLVMUseRef rhs ) => !( lhs == rhs );

        internal LLVMUseRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
