// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMSymbolIteratorRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMSymbolIteratorRef Zero = new LLVMSymbolIteratorRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMSymbolIteratorRef )
            {
                return Equals( ( LLVMSymbolIteratorRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMSymbolIteratorRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMSymbolIteratorRef lhs, LLVMSymbolIteratorRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMSymbolIteratorRef lhs, LLVMSymbolIteratorRef rhs ) => !lhs.Equals( rhs );

        internal LLVMSymbolIteratorRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
