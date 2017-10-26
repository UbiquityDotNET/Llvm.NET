// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMUseRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMUseRef Zero = new LLVMUseRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMUseRef )
            {
                return Equals( ( LLVMUseRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMUseRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMUseRef lhs, LLVMUseRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMUseRef lhs, LLVMUseRef rhs ) => !lhs.Equals( rhs );

        internal LLVMUseRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
