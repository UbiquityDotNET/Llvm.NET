// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMModuleRef
        : IEquatable<LLVMModuleRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMModuleRef r ) && r.Handle == Handle;

        public bool Equals( LLVMModuleRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMModuleRef lhs, LLVMModuleRef rhs )
            => EqualityComparer<LLVMModuleRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMModuleRef lhs, LLVMModuleRef rhs ) => !( lhs == rhs );

        internal LLVMModuleRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
