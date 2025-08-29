// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
