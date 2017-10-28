// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Native
{
    internal struct LLVMModuleRef
        : IEquatable<LLVMModuleRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMModuleRef r ) && r.Handle == Handle;

        public bool Equals( LLVMModuleRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMModuleRef lhs, LLVMModuleRef rhs )
            => EqualityComparer<LLVMModuleRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMModuleRef lhs, LLVMModuleRef rhs ) => !( lhs == rhs );

        public Context Context
        {
            get
            {
                if( Handle.IsNull( ) )
                {
                    return null;
                }

                var hContext = LLVMGetModuleContext( this );
                Debug.Assert( hContext != default, "Should not get a null pointer from LLVM" );
                return ( Context )hContext;
            }
        }

        internal LLVMModuleRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
