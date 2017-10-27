// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMNamedMDNodeRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMNamedMDNodeRef Zero = new LLVMNamedMDNodeRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMNamedMDNodeRef )
            {
                return Equals( ( LLVMNamedMDNodeRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMNamedMDNodeRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMNamedMDNodeRef lhs, LLVMNamedMDNodeRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMNamedMDNodeRef lhs, LLVMNamedMDNodeRef rhs ) => !lhs.Equals( rhs );

        internal LLVMNamedMDNodeRef(IntPtr pointer)
        {
            Handle = pointer;
        }
    }
}
