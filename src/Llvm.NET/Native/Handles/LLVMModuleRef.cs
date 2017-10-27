// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMModuleRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMModuleRef Zero = new LLVMModuleRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMModuleRef )
            {
                return Equals( ( LLVMModuleRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMModuleRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMModuleRef lhs, LLVMModuleRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMModuleRef lhs, LLVMModuleRef rhs ) => !lhs.Equals( rhs );

        internal LLVMModuleRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
