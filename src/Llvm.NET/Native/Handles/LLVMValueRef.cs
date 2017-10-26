// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMValueRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMValueRef Zero = new LLVMValueRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMValueRef )
            {
                return Equals( ( LLVMValueRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMValueRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMValueRef lhs, LLVMValueRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMValueRef lhs, LLVMValueRef rhs ) => !lhs.Equals( rhs );

        internal LLVMValueRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
