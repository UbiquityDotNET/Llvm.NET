// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Alloca instruction for allocating stack space</summary>
    /// <remarks>
    /// LLVM Mem2Reg pass will convert alloca locations to register for the
    /// entry block to the maximum extent possible.
    /// </remarks>
    /// <seealso href="xref:llvm_langref#alloca-instruction">LLVM alloca</seealso>
    public sealed class Alloca
        : UnaryInstruction
    {
        /// <summary>Gets the type of the alloca element</summary>
        /// <remarks>
        /// The <see cref="Ubiquity.NET.Llvm.Values.Value.NativeType"/> of an <see cref="Alloca"/>
        /// is always a pointer type, this provides the ElementType (e.g. the pointee type)
        /// for the alloca.
        /// </remarks>
        public ITypeRef ElementType => LLVMGetAllocatedType( Handle ).CreateType();

        /// <summary>Gets a value indicating whether this allocation is for an array</summary>
        public bool IsArrayAllocation => ElementCount > 1;

        /// <summary>Gets the array element count for this allocation</summary>
        /// <remarks>Allocations will always use at least 1 element, arrays are indicated if this value is >1</remarks>
        public Int64 ElementCount => Operands.GetOperand<ConstantInt>(0)?.SignExtendedValue ?? 1;

        internal Alloca( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
