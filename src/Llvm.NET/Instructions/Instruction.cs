// <copyright file="Instruction.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
{
    /// <summary>Exposes an LLVM Instruction</summary>
    public class Instruction
        : User
    {
        /// <summary>Gets the <see cref="BasicBlock"/> that contains this instruction</summary>
        public BasicBlock ContainingBlock => BasicBlock.FromHandle( LLVMGetInstructionParent( ValueHandle ) );

        /// <summary>Gets the LLVM opcode for the instruction</summary>
        public OpCode Opcode => ( OpCode )LLVMGetInstructionOpcode( ValueHandle );

        /// <summary>Gets a value indicating whether the opcode is for a memory access (<see cref="Alloca"/>, <see cref="Load"/>, <see cref="Store"/>)</summary>
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

        /// <summary>Gets or sets the alignment for the instruction</summary>
        /// <remarks>
        /// The alignment is always 0 for instructions other than <see cref="Alloca"/>,
        /// <see cref="Load"/>, <see cref="Store"/> that deal with memory accesses.
        /// Setting the alignment for other instructions results in an
        /// <see cref="InvalidOperationException"/>
        /// </remarks>
        public uint Alignment
        {
            get => IsMemoryAccess ? LLVMGetAlignment( ValueHandle ) : 0;

            set
            {
                if( !IsMemoryAccess )
                {
                    throw new InvalidOperationException( "Alignment can only be set for instructions dealing with memory read/write (alloca, load, store)" );
                }

                LLVMSetAlignment( ValueHandle, value );
            }
        }

        internal Instruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
