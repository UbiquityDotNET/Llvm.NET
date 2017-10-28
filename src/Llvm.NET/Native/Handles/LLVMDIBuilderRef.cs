// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMDIBuilderRef
        : IEquatable<LLVMDIBuilderRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMDIBuilderRef r ) && r.Handle == Handle;

        public bool Equals( LLVMDIBuilderRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMDIBuilderRef lhs, LLVMDIBuilderRef rhs )
            => EqualityComparer<LLVMDIBuilderRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMDIBuilderRef lhs, LLVMDIBuilderRef rhs ) => !( lhs == rhs );

        internal LLVMDIBuilderRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
