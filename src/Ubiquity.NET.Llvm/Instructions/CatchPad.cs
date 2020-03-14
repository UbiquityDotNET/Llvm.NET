﻿// -----------------------------------------------------------------------
// <copyright file="CatchPad.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Marks a <see cref="BasicBlock"/> as a catch handler</summary>
    /// <remarks>
    /// Like the <see cref="LandingPad"/>, instruction this must be the first non-phi instruction
    /// in the block.
    /// </remarks>
    /// <seealso href="xref:llvm_langref#catchpad-instruction">LLVM catchpad Instruction</seealso>
    /// <seealso href="xref:llvm_exception_handling#exception-handling-in-llvm">Exception Handling in LLVM</seealso>
    /// <seealso href="xref:llvm_exception_handling#wineh">Exception Handling using the Windows Runtime</seealso>
    public class CatchPad
        : FuncletPad
    {
        /// <summary>Gets or sets the <seealso cref="Ubiquity.NET.Llvm.Instructions.CatchSwitch"/> for this pad</summary>
        public CatchSwitch CatchSwitch
        {
            get => GetOperand<CatchSwitch>( -1 );
            set => SetOperand( -1, value );
        }

        internal CatchPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
