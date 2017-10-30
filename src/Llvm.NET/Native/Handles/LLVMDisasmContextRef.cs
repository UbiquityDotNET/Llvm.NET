// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMDisasmContextRef
        : IEquatable<LLVMDisasmContextRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMDisasmContextRef r ) && r.Handle == Handle;

        public bool Equals( LLVMDisasmContextRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMDisasmContextRef lhs, LLVMDisasmContextRef rhs )
            => EqualityComparer<LLVMDisasmContextRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMDisasmContextRef lhs, LLVMDisasmContextRef rhs ) => !( lhs == rhs );

        internal LLVMDisasmContextRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
