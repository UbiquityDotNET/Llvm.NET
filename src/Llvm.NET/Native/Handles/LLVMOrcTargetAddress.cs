// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMOrcTargetAddress
        : IEquatable<LLVMOrcTargetAddress>
    {
        public override int GetHashCode( ) => Address.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMOrcTargetAddress r ) && r.Address == Address;

        public bool Equals( LLVMOrcTargetAddress other ) => Address == other.Address;

        public static bool operator ==( LLVMOrcTargetAddress lhs, LLVMOrcTargetAddress rhs )
            => EqualityComparer<LLVMOrcTargetAddress>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMOrcTargetAddress lhs, LLVMOrcTargetAddress rhs ) => !( lhs == rhs );

        internal LLVMOrcTargetAddress( UInt64 value )
        {
            Address = value;
        }

        internal UInt64 Address { get; }
    }
}
