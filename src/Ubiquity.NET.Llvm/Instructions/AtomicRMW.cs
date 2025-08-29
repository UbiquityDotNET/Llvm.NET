// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Atomic Read-Modify-Write operation</summary>
    public enum AtomicRMWBinOp
    {
        /// <summary>Exchange operation</summary>
        Xchg = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXchg,

        /// <summary>Integer addition operation</summary>
        Add = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAdd,

        /// <summary>Integer subtraction</summary>
        Sub = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpSub,

        /// <summary>Bitwise AND</summary>
        And = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAnd,

        /// <summary>Bitwise NAND</summary>
        Nand = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpNand,

        /// <summary>Bitwise OR</summary>
        Or = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpOr,

        /// <summary>Bitwise XOR</summary>
        Xor = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXor,

        /// <summary>Max</summary>
        Max = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpMax,

        /// <summary>Min</summary>
        Min = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpMin,

        /// <summary>Unsigned Max</summary>
        UMax = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpUMax,

        /// <summary>Unsigned Min</summary>
        UMin = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpUMin,

        /// <summary>Floating point addition</summary>
        FAdd = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpFAdd,

        /// <summary>Floating point subtraction</summary>
        FSub = LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpFSub
    }

    /// <summary>Atomic Read-Modify-Write instruction</summary>
    /// <seealso href="xref:llvm_langref#atomicrmw-instruction">LLVM atomicrmw instruction</seealso>
    public sealed class AtomicRMW
            : Instruction
    {
        /// <summary>Gets or sets the kind of atomic operation for this instruction</summary>
        public AtomicRMWBinOp Kind
        {
            get => (AtomicRMWBinOp)LLVMGetAtomicRMWBinOp( Handle );
            set => LLVMSetAtomicRMWBinOp( Handle, (LLVMAtomicRMWBinOp)value.ThrowIfNotDefined() );
        }

        internal AtomicRMW( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
