// <copyright file="CatchReturn.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Catch return instruction</summary>
    public class CatchReturn
        : Terminator
    {
        /// <summary>Gets the <see cref="CatchPad"/> instruction associated with this <see cref="CatchReturn"/></summary>
        public CatchPad CatchPad => GetOperand<CatchPad>( 0 );

        /// <summary>Gets the Successor <see cref="BasicBlock"/>s for this instruction</summary>
        public IReadOnlyList<BasicBlock> Successors => new List<BasicBlock> { GetOperand<BasicBlock>( 1 ) };

        /// <summary>Gets the <see cref="CatchSwitch.ParentPad"/> property from the <see cref="Llvm.NET.Instructions.CatchPad.CatchSwitch"/>
        /// of the <see cref="CatchReturn.CatchPad"/> property</summary>
        public Value CatchSwitchParentPad => CatchPad.CatchSwitch.ParentPad;

        internal CatchReturn( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
