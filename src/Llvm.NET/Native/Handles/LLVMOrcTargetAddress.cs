// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMOrcTargetAddress
    {
        public static LLVMOrcTargetAddress Zero = new LLVMOrcTargetAddress( 0 );

        public override int GetHashCode( ) => Value.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMOrcTargetAddress )
            {
                return Equals( ( LLVMOrcTargetAddress )obj );
            }

            if( obj is IntPtr )
            {
                return Value.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMOrcTargetAddress other ) => Value == other.Value;

        public static bool operator ==( LLVMOrcTargetAddress lhs, LLVMOrcTargetAddress rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMOrcTargetAddress lhs, LLVMOrcTargetAddress rhs ) => !lhs.Equals( rhs );

        internal LLVMOrcTargetAddress( ulong value )
        {
            Value = value;
        }

        internal ulong Value { get; }
    }
}
