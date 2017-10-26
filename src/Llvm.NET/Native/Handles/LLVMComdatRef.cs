// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMComdatRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMComdatRef Zero = new LLVMComdatRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMComdatRef )
            {
                return Equals( ( LLVMComdatRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMComdatRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMComdatRef lhs, LLVMComdatRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMComdatRef lhs, LLVMComdatRef rhs ) => !lhs.Equals( rhs );

        internal LLVMComdatRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
