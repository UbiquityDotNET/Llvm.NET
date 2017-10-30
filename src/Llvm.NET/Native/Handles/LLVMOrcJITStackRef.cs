// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMOrcJITStackRef
        : IEquatable<LLVMOrcJITStackRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMOrcJITStackRef r ) && r.Handle == Handle;

        public bool Equals( LLVMOrcJITStackRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMOrcJITStackRef lhs, LLVMOrcJITStackRef rhs )
            => EqualityComparer<LLVMOrcJITStackRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMOrcJITStackRef lhs, LLVMOrcJITStackRef rhs ) => !( lhs == rhs );

        internal LLVMOrcJITStackRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
