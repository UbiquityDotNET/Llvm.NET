// <copyright file="Instruction.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Exposes an LLVM Instruction</summary>
    public class Instruction
        : User
    {
        /// <summary>Block that contains this instruction</summary>
        public BasicBlock ContainingBlock => BasicBlock.FromHandle( NativeMethods.GetInstructionParent( ValueHandle ) );

        /// <summary>Gets the LLVM opcode for the instruction</summary>
        public OpCode Opcode => ( OpCode )NativeMethods.GetInstructionOpcode( ValueHandle );

        /// <summary>Flag to indicate if the opcode is for a memory access <see cref="Alloca"/>, <see cref="Load"/>, <see cref="Store"/></summary>
        public bool IsMemoryAccess
        {
            get
            {
                var opCode = Opcode;
                return opCode == OpCode.Alloca
                    || opCode == OpCode.Load
                    || opCode == OpCode.Store;
            }
        }

        /// <summary>Alignment for the instruction</summary>
        /// <remarks>
        /// The alignment is always 0 for instructions other than Alloca, Load, and Store
        /// that deal with memory accesses. Setting the alignment for other instructions
        /// results in an InvalidOperationException()
        /// </remarks>
        public uint Alignment
        {
            get => IsMemoryAccess ? NativeMethods.GetAlignment( ValueHandle ) : 0;

            set
            {
                if( !IsMemoryAccess )
                {
                    throw new InvalidOperationException( "Alignment can only be set for instructions dealing with memory read/write (alloca, load, store)" );
                }

                NativeMethods.SetAlignment( ValueHandle, value );
            }
        }

        internal Instruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
