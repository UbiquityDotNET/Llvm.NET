// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMDisasmContextRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMDisasmContextRef Zero = new LLVMDisasmContextRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMDisasmContextRef )
            {
                return Equals( ( LLVMDisasmContextRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMDisasmContextRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMDisasmContextRef lhs, LLVMDisasmContextRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMDisasmContextRef lhs, LLVMDisasmContextRef rhs ) => !lhs.Equals( rhs );

        internal LLVMDisasmContextRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
