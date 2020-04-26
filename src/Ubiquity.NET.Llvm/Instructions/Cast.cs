﻿// -----------------------------------------------------------------------
// <copyright file="Cast.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Base class for cast instructions</summary>
    public class Cast
        : UnaryInstruction
    {
        /// <summary>Gets the source type of the cast</summary>
        public ITypeRef FromType => Operands.GetOperand<Value>( 0 )!.NativeType;

        /// <summary>Gets the destination type of the cast</summary>
        public ITypeRef ToType => NativeType;

        internal Cast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
