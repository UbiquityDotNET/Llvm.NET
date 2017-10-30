// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMNamedMDNodeRef
        : IEquatable<LLVMNamedMDNodeRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMNamedMDNodeRef r ) && r.Handle == Handle;

        public bool Equals( LLVMNamedMDNodeRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMNamedMDNodeRef lhs, LLVMNamedMDNodeRef rhs )
            => EqualityComparer<LLVMNamedMDNodeRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMNamedMDNodeRef lhs, LLVMNamedMDNodeRef rhs ) => !( lhs == rhs );

        internal LLVMNamedMDNodeRef(IntPtr pointer)
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
