// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMTargetRef
        : IEquatable<LLVMTargetRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMTargetRef r ) && r.Handle == Handle;

        public bool Equals( LLVMTargetRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTargetRef lhs, LLVMTargetRef rhs )
            => EqualityComparer<LLVMTargetRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMTargetRef lhs, LLVMTargetRef rhs ) => !( lhs == rhs );

        internal LLVMTargetRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
