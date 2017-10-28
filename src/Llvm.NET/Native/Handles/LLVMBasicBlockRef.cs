// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMBasicBlockRef
        : IEquatable<LLVMBasicBlockRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMBasicBlockRef r ) && r.Handle == Handle;

        public bool Equals( LLVMBasicBlockRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMBasicBlockRef lhs, LLVMBasicBlockRef rhs )
            => EqualityComparer<LLVMBasicBlockRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMBasicBlockRef lhs, LLVMBasicBlockRef rhs ) => !( lhs == rhs );

        internal LLVMBasicBlockRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
