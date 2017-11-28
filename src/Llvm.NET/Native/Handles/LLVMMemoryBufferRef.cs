// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
     // TODO: replace with LlvmObject impl so that MemoryBuffer can remove the IDisposable interface and function like any other
     // garbage collected type in .NET
     internal struct LLVMMemoryBufferRef
        : IEquatable<LLVMMemoryBufferRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMMemoryBufferRef r ) && r.Handle == Handle;

        public bool Equals( LLVMMemoryBufferRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMMemoryBufferRef lhs, LLVMMemoryBufferRef rhs )
            => EqualityComparer<LLVMMemoryBufferRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMMemoryBufferRef lhs, LLVMMemoryBufferRef rhs ) => !( lhs == rhs );

        internal LLVMMemoryBufferRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
