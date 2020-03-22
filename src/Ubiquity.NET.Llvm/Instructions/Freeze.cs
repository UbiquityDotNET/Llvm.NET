// -----------------------------------------------------------------------
// <copyright file="Freeze.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Freeze a poison or undef value</summary>
    public class Freeze
        : UnaryInstruction
    {
        /// <summary>Gets the value this instruction freezes</summary>
        public Value Value => GetOperand<Value>( 0 );

        internal Freeze( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
