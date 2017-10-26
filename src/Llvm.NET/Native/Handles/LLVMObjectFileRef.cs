// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMObjectFileRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMObjectFileRef Zero = new LLVMObjectFileRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMObjectFileRef )
            {
                return Equals( ( LLVMObjectFileRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMObjectFileRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMObjectFileRef lhs, LLVMObjectFileRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMObjectFileRef lhs, LLVMObjectFileRef rhs ) => !lhs.Equals( rhs );

        internal LLVMObjectFileRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
