// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMModuleProviderRef
        : IEquatable<LLVMModuleProviderRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMModuleProviderRef r ) && r.Handle == Handle;

        public bool Equals( LLVMModuleProviderRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMModuleProviderRef lhs, LLVMModuleProviderRef rhs )
            => EqualityComparer<LLVMModuleProviderRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMModuleProviderRef lhs, LLVMModuleProviderRef rhs ) => !( lhs == rhs );

        internal LLVMModuleProviderRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
