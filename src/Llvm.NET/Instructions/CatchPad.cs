// <copyright file="CatchPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Marks a <see cref="BasicBlock"/> as a catch handler</summary>
    /// <remarks>
    /// Like the <see cref="LandingPad"/>, instruction this must be the first non-phi instruction
    /// in the block.
    /// </remarks>
    /// <seealso href="xref:llvm_langref#catchpad-instruction">LLVM 'catchpad' Instruction</seealso>
    /// <seealso href="xref:llvm_releases_docs/ExceptionHandling.html">Exception Handling in LLVM</seealso>
    /// <seealso href="xref:llvm_releases_docs/ExceptionHandle.html#wineh">Exception Handling using the Windows Runtime</seealso>
    public class CatchPad
        : FuncletPad
    {
        /// <summary>Gets the <seealso cref="Llvm.NET.Instructions.CatchSwitch"/> for this pad</summary>
        public CatchSwitch CatchSwitch => GetOperand<CatchSwitch>( -1 );

        /* TODO: Switch {set;} */

        internal CatchPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
