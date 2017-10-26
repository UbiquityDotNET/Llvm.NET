// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMPassManagerBuilderRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMPassManagerBuilderRef Zero = new LLVMPassManagerBuilderRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMPassManagerBuilderRef )
            {
                return Equals( ( LLVMPassManagerBuilderRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMPassManagerBuilderRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMPassManagerBuilderRef lhs, LLVMPassManagerBuilderRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMPassManagerBuilderRef lhs, LLVMPassManagerBuilderRef rhs ) => !lhs.Equals( rhs );

        internal LLVMPassManagerBuilderRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
