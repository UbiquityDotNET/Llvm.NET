// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMTargetRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMTargetRef Zero = new LLVMTargetRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMTargetRef )
            {
                return Equals( ( LLVMTargetRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMTargetRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTargetRef lhs, LLVMTargetRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMTargetRef lhs, LLVMTargetRef rhs ) => !lhs.Equals( rhs );

        internal LLVMTargetRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
