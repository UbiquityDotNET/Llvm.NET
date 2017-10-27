// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMTargetDataRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMTargetDataRef Zero = new LLVMTargetDataRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMTargetDataRef )
            {
                return Equals( ( LLVMTargetDataRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMTargetDataRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTargetDataRef lhs, LLVMTargetDataRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMTargetDataRef lhs, LLVMTargetDataRef rhs ) => !lhs.Equals( rhs );

        internal LLVMTargetDataRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
