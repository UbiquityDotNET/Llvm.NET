// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMDiagnosticInfoRef
        : IEquatable<LLVMDiagnosticInfoRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMDiagnosticInfoRef r ) && r.Handle == Handle;

        public bool Equals( LLVMDiagnosticInfoRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMDiagnosticInfoRef lhs, LLVMDiagnosticInfoRef rhs )
            => EqualityComparer<LLVMDiagnosticInfoRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMDiagnosticInfoRef lhs, LLVMDiagnosticInfoRef rhs ) => !( lhs == rhs );

        internal LLVMDiagnosticInfoRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
