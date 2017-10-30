// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMObjectFileRef
        : IEquatable<LLVMObjectFileRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMObjectFileRef r ) && r.Handle == Handle;

        public bool Equals( LLVMObjectFileRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMObjectFileRef lhs, LLVMObjectFileRef rhs )
            => EqualityComparer<LLVMObjectFileRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMObjectFileRef lhs, LLVMObjectFileRef rhs ) => !( lhs == rhs );

        internal LLVMObjectFileRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
