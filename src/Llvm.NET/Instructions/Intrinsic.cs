﻿// <copyright file="Intrinsic.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Instructions
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
            return LLVMLookupInstrinsicId( name );
        }

        internal Intrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
