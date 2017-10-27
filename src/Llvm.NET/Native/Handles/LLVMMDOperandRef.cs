// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMMDOperandRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMMDOperandRef Zero = new LLVMMDOperandRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMMDOperandRef )
            {
                return Equals( ( LLVMMDOperandRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMMDOperandRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMMDOperandRef lhs, LLVMMDOperandRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMMDOperandRef lhs, LLVMMDOperandRef rhs ) => !lhs.Equals( rhs );

        internal LLVMMDOperandRef(IntPtr pointer)
        {
            Handle = pointer;
        }
    }
}
