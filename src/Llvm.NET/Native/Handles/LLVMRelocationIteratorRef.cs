// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMRelocationIteratorRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMRelocationIteratorRef Zero = new LLVMRelocationIteratorRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMRelocationIteratorRef )
            {
                return Equals( ( LLVMRelocationIteratorRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMRelocationIteratorRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMRelocationIteratorRef lhs, LLVMRelocationIteratorRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMRelocationIteratorRef lhs, LLVMRelocationIteratorRef rhs ) => !lhs.Equals( rhs );

        internal LLVMRelocationIteratorRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
