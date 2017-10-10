// <copyright file="LLVMMetadataRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal partial struct LLVMMetadataRef
        : IEquatable<LLVMMetadataRef>
    {
        public static LLVMMetadataRef Zero = new LLVMMetadataRef( IntPtr.Zero );

        public override int GetHashCode( ) => Pointer.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMMetadataRef )
            {
                return Equals( ( LLVMMetadataRef )obj );
            }

            if( obj is IntPtr )
            {
                return Pointer.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMMetadataRef other ) => Pointer == other.Pointer;

        public static bool operator ==( LLVMMetadataRef lhs, LLVMMetadataRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMMetadataRef lhs, LLVMMetadataRef rhs ) => !lhs.Equals( rhs );
    }
}
