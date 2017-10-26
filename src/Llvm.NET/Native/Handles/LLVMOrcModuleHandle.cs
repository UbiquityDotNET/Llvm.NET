// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMOrcModuleHandle
    {
        public static LLVMOrcModuleHandle Zero = new LLVMOrcModuleHandle( 0 );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMOrcModuleHandle )
            {
                return Equals( ( LLVMOrcModuleHandle )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMOrcModuleHandle other ) => Handle == other.Handle;

        public static bool operator ==( LLVMOrcModuleHandle lhs, LLVMOrcModuleHandle rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMOrcModuleHandle lhs, LLVMOrcModuleHandle rhs ) => !lhs.Equals( rhs );

        internal int Handle { get; }

        internal LLVMOrcModuleHandle( int value )
        {
            Handle = value;
        }
    }
}
