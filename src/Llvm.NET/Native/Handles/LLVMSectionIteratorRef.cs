// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMSectionIteratorRef
        : IEquatable<LLVMSectionIteratorRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMSectionIteratorRef r ) && r.Handle == Handle;

        public bool Equals( LLVMSectionIteratorRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMSectionIteratorRef lhs, LLVMSectionIteratorRef rhs )
            => EqualityComparer<LLVMSectionIteratorRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMSectionIteratorRef lhs, LLVMSectionIteratorRef rhs ) => !( lhs == rhs );

        internal LLVMSectionIteratorRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
