﻿// <copyright file="ConstantInt.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Represents an arbitrary bit width integer constant in LLVM</summary>
    /// <remarks>
    /// Note - for integers, in LLVM, signed or unsigned is not part of the type of
    /// the integer. The distinction between them is determined entirely by the
    /// instructions used on the integer values.
    /// </remarks>
    public class ConstantInt
        : Constant
    {
        /// <summary>Gets the value of the constant zero extended to a 64 bit value</summary>
        public UInt64 ZeroExtendedValue => NativeMethods.ConstIntGetZExtValue( ValueHandle );

        /// <summary>Gets the value of the constant sign extended to a 64 bit value</summary>
        public Int64 SignExtendedValue => NativeMethods.ConstIntGetSExtValue( ValueHandle );

        internal ConstantInt( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
