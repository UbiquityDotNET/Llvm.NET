// <copyright file="LLVMMetadataRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    internal struct LLVMMetadataRef
        : IEquatable<LLVMMetadataRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMMetadataRef r ) && r.Handle == Handle;

        public bool Equals( LLVMMetadataRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMMetadataRef lhs, LLVMMetadataRef rhs )
            => EqualityComparer<LLVMMetadataRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMMetadataRef lhs, LLVMMetadataRef rhs ) => !( lhs == rhs );

        public Context Context
        {
            get
            {
                if( Handle == default )
                {
                    return null;
                }

                var hContext = LLVMGetNodeContext( this );
                Debug.Assert( hContext != default, "Should not get a null pointer from LLVM" );
                return ContextCache.GetContextFor( hContext );
            }
        }

        internal LLVMMetadataRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMContextAlias LLVMGetNodeContext( LLVMMetadataRef /*MDNode*/ node );

        private readonly IntPtr Handle;
    }
}
