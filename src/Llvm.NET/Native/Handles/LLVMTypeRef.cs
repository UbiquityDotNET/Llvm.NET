// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMTypeRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMTypeRef Zero = new LLVMTypeRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMTypeRef )
            {
                return Equals( ( LLVMTypeRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMTypeRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTypeRef lhs, LLVMTypeRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMTypeRef lhs, LLVMTypeRef rhs ) => !lhs.Equals( rhs );

        internal LLVMTypeRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
