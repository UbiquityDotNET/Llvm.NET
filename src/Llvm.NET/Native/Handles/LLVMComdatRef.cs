// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMComdatRef
        : IEquatable<LLVMComdatRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMComdatRef r ) && r.Handle == Handle;

        public bool Equals( LLVMComdatRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMComdatRef lhs, LLVMComdatRef rhs )
            => EqualityComparer<LLVMComdatRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMComdatRef lhs, LLVMComdatRef rhs ) => !( lhs == rhs );

        internal LLVMComdatRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
