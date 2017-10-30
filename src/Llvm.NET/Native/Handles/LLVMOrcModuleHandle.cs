// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMOrcModuleHandle
        : IEquatable<LLVMOrcModuleHandle>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMOrcModuleHandle r ) && r.Handle == Handle;

        public bool Equals( LLVMOrcModuleHandle other ) => Handle == other.Handle;

        public static bool operator ==( LLVMOrcModuleHandle lhs, LLVMOrcModuleHandle rhs )
            => EqualityComparer<LLVMOrcModuleHandle>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMOrcModuleHandle lhs, LLVMOrcModuleHandle rhs ) => !( lhs == rhs );

        internal LLVMOrcModuleHandle( UInt32 value )
        {
            Handle = value;
        }

        private readonly UInt32 Handle;
    }
}
