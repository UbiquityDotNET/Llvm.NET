// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMTypeRef
        : IEquatable<LLVMTypeRef>
    {
        public bool IsNull => Handle.IsNull( );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMTypeRef r ) && r.Handle == Handle;

        public bool Equals( LLVMTypeRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTypeRef lhs, LLVMTypeRef rhs )
            => EqualityComparer<LLVMTypeRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMTypeRef lhs, LLVMTypeRef rhs ) => !( lhs == rhs );

        internal LLVMTypeRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
