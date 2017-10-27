// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMMemoryBufferRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMMemoryBufferRef Zero = new LLVMMemoryBufferRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMMemoryBufferRef )
            {
                return Equals( ( LLVMMemoryBufferRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMMemoryBufferRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMMemoryBufferRef lhs, LLVMMemoryBufferRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMMemoryBufferRef lhs, LLVMMemoryBufferRef rhs ) => !lhs.Equals( rhs );

        internal LLVMMemoryBufferRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
