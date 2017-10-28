// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMTargetLibraryInfoRef
        : IEquatable<LLVMTargetLibraryInfoRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMTargetLibraryInfoRef r ) && r.Handle == Handle;

        public bool Equals( LLVMTargetLibraryInfoRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTargetLibraryInfoRef lhs, LLVMTargetLibraryInfoRef rhs )
            => EqualityComparer<LLVMTargetLibraryInfoRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMTargetLibraryInfoRef lhs, LLVMTargetLibraryInfoRef rhs ) => !( lhs == rhs );

        internal LLVMTargetLibraryInfoRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
