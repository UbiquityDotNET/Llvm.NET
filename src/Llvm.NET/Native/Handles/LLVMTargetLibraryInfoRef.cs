// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMTargetLibraryInfoRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMTargetLibraryInfoRef Zero = new LLVMTargetLibraryInfoRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMTargetLibraryInfoRef )
            {
                return Equals( ( LLVMTargetLibraryInfoRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMTargetLibraryInfoRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTargetLibraryInfoRef lhs, LLVMTargetLibraryInfoRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMTargetLibraryInfoRef lhs, LLVMTargetLibraryInfoRef rhs ) => !lhs.Equals( rhs );

        internal LLVMTargetLibraryInfoRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
