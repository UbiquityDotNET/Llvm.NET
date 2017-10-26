// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMDiagnosticInfoRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMDiagnosticInfoRef Zero = new LLVMDiagnosticInfoRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMDiagnosticInfoRef )
            {
                return Equals( ( LLVMDiagnosticInfoRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMDiagnosticInfoRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMDiagnosticInfoRef lhs, LLVMDiagnosticInfoRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMDiagnosticInfoRef lhs, LLVMDiagnosticInfoRef rhs ) => !lhs.Equals( rhs );

        internal LLVMDiagnosticInfoRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}

#pragma warning restore SA1600 // Elements must be documented
