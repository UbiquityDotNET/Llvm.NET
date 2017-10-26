// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMExecutionEngineRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMExecutionEngineRef Zero = new LLVMExecutionEngineRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMExecutionEngineRef )
            {
                return Equals( ( LLVMExecutionEngineRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMExecutionEngineRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMExecutionEngineRef lhs, LLVMExecutionEngineRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMExecutionEngineRef lhs, LLVMExecutionEngineRef rhs ) => !lhs.Equals( rhs );

        internal LLVMExecutionEngineRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
