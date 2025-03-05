// -----------------------------------------------------------------------
// <copyright file="Intrinsic.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>base class for calls to LLVM intrinsic functions</summary>
    public class Intrinsic
        : CallInstruction
    {
        /// <summary>Looks up the LLVM intrinsic ID from it's name</summary>
        /// <param name="name">Name of the intrinsic</param>
        /// <returns>Intrinsic ID or 0 if the name does not correspond with an intrinsic function</returns>
        public static UInt32 LookupId( string name )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            return LLVMLookupIntrinsicID( name, name.Length );
        }

        internal Intrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
