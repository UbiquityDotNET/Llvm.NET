// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMExecutionEngineRef
        : IEquatable<LLVMExecutionEngineRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMExecutionEngineRef r ) && r.Handle == Handle;

        public bool Equals( LLVMExecutionEngineRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMExecutionEngineRef lhs, LLVMExecutionEngineRef rhs )
            => EqualityComparer<LLVMExecutionEngineRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMExecutionEngineRef lhs, LLVMExecutionEngineRef rhs ) => !( lhs == rhs );

        internal LLVMExecutionEngineRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
