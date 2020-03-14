﻿// -----------------------------------------------------------------------
// <copyright file="PhiNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>PHI node instruction</summary>
    public class PhiNode
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
            additionalIncoming.ValidateNotNull( nameof( additionalIncoming ) );

            var allIncoming = additionalIncoming.Prepend( firstIncoming );

            LLVMValueRef[ ] llvmValues = allIncoming.Select( vb => vb.Value.ValueHandle ).ToArray( );
            LLVMBasicBlockRef[ ] llvmBlocks = allIncoming.Select( vb => vb.Block.BlockHandle ).ToArray( );

            LLVMAddIncoming( ValueHandle, llvmValues, llvmBlocks, ( uint )llvmValues.Length );
        }

        internal PhiNode( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
