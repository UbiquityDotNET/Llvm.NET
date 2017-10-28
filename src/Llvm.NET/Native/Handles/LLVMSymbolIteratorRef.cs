// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMSymbolIteratorRef
        : IEquatable<LLVMSymbolIteratorRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMSymbolIteratorRef r ) && r.Handle == Handle;

        public bool Equals( LLVMSymbolIteratorRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMSymbolIteratorRef lhs, LLVMSymbolIteratorRef rhs )
            => EqualityComparer<LLVMSymbolIteratorRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMSymbolIteratorRef lhs, LLVMSymbolIteratorRef rhs ) => !( lhs == rhs );

        internal LLVMSymbolIteratorRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
