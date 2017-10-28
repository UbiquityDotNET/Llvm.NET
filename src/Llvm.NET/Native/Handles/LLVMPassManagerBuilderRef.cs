// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMPassManagerBuilderRef
        : IEquatable<LLVMPassManagerBuilderRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMPassManagerBuilderRef r ) && r.Handle == Handle;

        public bool Equals( LLVMPassManagerBuilderRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMPassManagerBuilderRef lhs, LLVMPassManagerBuilderRef rhs )
            => EqualityComparer<LLVMPassManagerBuilderRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMPassManagerBuilderRef lhs, LLVMPassManagerBuilderRef rhs ) => !( lhs == rhs );

        internal LLVMPassManagerBuilderRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
