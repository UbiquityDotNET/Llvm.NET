﻿// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMOrcJITStackRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMOrcJITStackRef Zero = new LLVMOrcJITStackRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMOrcJITStackRef )
            {
                return Equals( ( LLVMOrcJITStackRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMOrcJITStackRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMOrcJITStackRef lhs, LLVMOrcJITStackRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMOrcJITStackRef lhs, LLVMOrcJITStackRef rhs ) => !lhs.Equals( rhs );

        internal LLVMOrcJITStackRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}