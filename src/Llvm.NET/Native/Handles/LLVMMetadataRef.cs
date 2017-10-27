// <copyright file="LLVMMetadataRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMMetadataRef
        : IEquatable<LLVMMetadataRef>
        , ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMMetadataRef Zero = new LLVMMetadataRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMMetadataRef )
            {
                return Equals( ( LLVMMetadataRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMMetadataRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMMetadataRef lhs, LLVMMetadataRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMMetadataRef lhs, LLVMMetadataRef rhs ) => !lhs.Equals( rhs );

        internal LLVMMetadataRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
