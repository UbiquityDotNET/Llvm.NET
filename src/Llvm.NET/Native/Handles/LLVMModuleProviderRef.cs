// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMModuleProviderRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMModuleProviderRef Zero = new LLVMModuleProviderRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMModuleProviderRef )
            {
                return Equals( ( LLVMModuleProviderRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMModuleProviderRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMModuleProviderRef lhs, LLVMModuleProviderRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMModuleProviderRef lhs, LLVMModuleProviderRef rhs ) => !lhs.Equals( rhs );

        internal LLVMModuleProviderRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
