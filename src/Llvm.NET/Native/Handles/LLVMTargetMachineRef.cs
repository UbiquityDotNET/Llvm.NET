// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMTargetMachineRef
        : IEquatable<LLVMTargetMachineRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj ) => !( obj is null ) && ( obj is LLVMTargetMachineRef r ) && r.Handle == Handle;

        public bool Equals( LLVMTargetMachineRef other ) => Handle == other.Handle;

        public static bool operator ==( LLVMTargetMachineRef lhs, LLVMTargetMachineRef rhs )
            => EqualityComparer<LLVMTargetMachineRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMTargetMachineRef lhs, LLVMTargetMachineRef rhs ) => !( lhs == rhs );

        internal LLVMTargetMachineRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
