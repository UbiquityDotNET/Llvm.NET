// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 2.17941.31104.49410
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Llvm.NET.Interop
{
    [GeneratedCode("LlvmBindingsGenerator","2.17941.31104.49410")]
    public struct LLVMComdatRef
        : IEquatable<LLVMComdatRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
            => !( obj is null )
             && ( obj is LLVMComdatRef r )
             && ( r.Handle == Handle );

        public bool Equals( LLVMComdatRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMComdatRef lhs, LLVMComdatRef rhs )
            => EqualityComparer<LLVMComdatRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMComdatRef lhs, LLVMComdatRef rhs ) => !( lhs == rhs );

        internal LLVMComdatRef( IntPtr p )
        {
            Handle = p;
        }

        private readonly IntPtr Handle;
    }
}
