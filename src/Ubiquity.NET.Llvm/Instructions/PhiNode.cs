// -----------------------------------------------------------------------
// <copyright file="PhiNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>PHI node instruction</summary>
    public sealed class PhiNode
        : Instruction
    {
        /// <summary>Adds an incoming value and block to this <see cref="PhiNode"/></summary>
        /// <param name="value">Value from <paramref name="srcBlock"/></param>
        /// <param name="srcBlock">Incoming block</param>
        public void AddIncoming( Value value, BasicBlock srcBlock )
        {
            AddIncoming( (value, srcBlock) );
        }

        /// <summary>Adds incoming blocks and values to this <see cref="PhiNode"/></summary>
        /// <param name="firstIncoming">first incoming value and block</param>
        /// <param name="additionalIncoming">additional values and blocks</param>
        public void AddIncoming( (Value Value, BasicBlock Block) firstIncoming, params (Value Value, BasicBlock Block)[ ] additionalIncoming )
        {
            ArgumentNullException.ThrowIfNull( additionalIncoming );

            var allIncoming = additionalIncoming.Prepend( firstIncoming );

            LLVMValueRef[ ] llvmValues = [ .. allIncoming.Select( vb => vb.Value.Handle ) ];
            LLVMBasicBlockRef[ ] llvmBlocks = [ .. allIncoming.Select( vb => vb.Block.BlockHandle ) ];

            LLVMAddIncoming( Handle, llvmValues, llvmBlocks, ( uint )llvmValues.Length );
        }

        internal PhiNode( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
