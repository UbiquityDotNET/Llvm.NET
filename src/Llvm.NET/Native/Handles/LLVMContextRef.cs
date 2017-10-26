// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMContextRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMContextRef Zero = new LLVMContextRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMContextRef )
            {
                return Equals( ( LLVMContextRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMContextRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMContextRef lhs, LLVMContextRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMContextRef lhs, LLVMContextRef rhs ) => !lhs.Equals( rhs );

        internal LLVMContextRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
