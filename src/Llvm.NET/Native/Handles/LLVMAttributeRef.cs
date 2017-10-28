// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMAttributeRef
        : IEquatable<LLVMAttributeRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && (obj is LLVMAttributeRef r) && r.Handle == Handle;

        public bool Equals( LLVMAttributeRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMAttributeRef lhs, LLVMAttributeRef rhs )
            => EqualityComparer<LLVMAttributeRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMAttributeRef lhs, LLVMAttributeRef rhs ) => !( lhs == rhs );

        internal LLVMAttributeRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
