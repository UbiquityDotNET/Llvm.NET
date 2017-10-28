// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    internal struct LLVMTypeRef
        : IEquatable<LLVMTypeRef>
    {
        public bool IsNull => Handle.IsNull( );

        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMTypeRef r ) && r.Handle == Handle;

        public bool Equals( LLVMTypeRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTypeRef lhs, LLVMTypeRef rhs )
            => EqualityComparer<LLVMTypeRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMTypeRef lhs, LLVMTypeRef rhs ) => !( lhs == rhs );

        public Context Context
        {
            get
            {
                if( Handle.IsNull( ) )
                {
                    return null;
                }

                var hContext = LLVMGetTypeContext( this );
                Debug.Assert( hContext != default, "Should not get a null pointer from LLVM" );
                return ( Context )hContext;
            }
        }

        internal LLVMTypeRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMContextAlias LLVMGetTypeContext( LLVMTypeRef @Ty );

        private readonly IntPtr Handle;
    }
}
