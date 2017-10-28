// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMTargetDataRef
        : IEquatable<LLVMTargetDataRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMTargetDataRef r ) && r.Handle == Handle;

        public bool Equals( LLVMTargetDataRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTargetDataRef lhs, LLVMTargetDataRef rhs )
            => EqualityComparer<LLVMTargetDataRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMTargetDataRef lhs, LLVMTargetDataRef rhs ) => !( lhs == rhs );

        internal LLVMTargetDataRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
