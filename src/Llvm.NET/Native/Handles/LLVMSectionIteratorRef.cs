// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMSectionIteratorRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMSectionIteratorRef Zero = new LLVMSectionIteratorRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMSectionIteratorRef )
            {
                return Equals( ( LLVMSectionIteratorRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMSectionIteratorRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMSectionIteratorRef lhs, LLVMSectionIteratorRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMSectionIteratorRef lhs, LLVMSectionIteratorRef rhs ) => !lhs.Equals( rhs );

        internal LLVMSectionIteratorRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
