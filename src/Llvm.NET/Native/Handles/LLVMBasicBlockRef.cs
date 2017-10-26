﻿// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

/* NOTE: While this code was originally generated from Clang based tool parsing the LLVM headers
// it was modified extensively since then to use more correct marsahlling attributes as well as
// custom marshalling for the specially allocated strings used by LLVM. This is not auto generated.
// This represents a low level interop P/Invoke of the standard LLVM-C API. Additional C-APIs are
// added by the LibLlvm project and those are declared in the CustomGenerated. (Which is also
// maintained manually now)
*/

using System;

namespace Llvm.NET.Native
{
    internal struct LLVMBasicBlockRef
        : ILlvmHandle
    {
        public IntPtr Handle { get; }

        public static LLVMBasicBlockRef Zero = new LLVMBasicBlockRef( IntPtr.Zero );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is LLVMBasicBlockRef )
            {
                return Equals( ( LLVMBasicBlockRef )obj );
            }

            if( obj is IntPtr )
            {
                return Handle.Equals( obj );
            }

            return base.Equals( obj );
        }

        public bool Equals( LLVMBasicBlockRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMBasicBlockRef lhs, LLVMBasicBlockRef rhs ) => lhs.Equals( rhs );

        public static bool operator !=( LLVMBasicBlockRef lhs, LLVMBasicBlockRef rhs ) => !lhs.Equals( rhs );

        internal LLVMBasicBlockRef( IntPtr pointer )
        {
            Handle = pointer;
        }
    }
}
