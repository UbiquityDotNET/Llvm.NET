// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
    public sealed class CatchPad
        : FuncletPad
    {
        /// <summary>Gets or sets the <seealso cref="Ubiquity.NET.Llvm.Instructions.CatchSwitch"/> for this pad</summary>
        [DisallowNull]
        public CatchSwitch CatchSwitch
        {
            get => Operands.GetOperand<CatchSwitch>( ^1 )!;
            set => Operands[ ^1 ] = value.ThrowIfNull();
        }

        internal CatchPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
