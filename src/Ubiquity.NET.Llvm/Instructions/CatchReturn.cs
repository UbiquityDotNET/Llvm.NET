// -----------------------------------------------------------------------
// <copyright file="CatchReturn.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Catch return instruction</summary>
    public sealed class CatchReturn
        : Terminator
    {
        /// <summary>Gets the <see cref="CatchPad"/> instruction associated with this <see cref="CatchReturn"/></summary>
        public CatchPad CatchPad => Operands.GetOperand<CatchPad>( 0 )!;

        /// <summary>Gets the <see cref="CatchSwitch.ParentPad"/> property from the <see cref="Ubiquity.NET.Llvm.Instructions.CatchPad.CatchSwitch"/>
        /// of the <see cref="CatchReturn.CatchPad"/> property</summary>
        public Value CatchSwitchParentPad => CatchPad.CatchSwitch.ParentPad;

        internal CatchReturn( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
