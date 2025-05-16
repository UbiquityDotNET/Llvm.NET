// -----------------------------------------------------------------------
// <copyright file="Load.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to read from memory</summary>
    /// <seealso href="xref:llvm_langref#load-instruction">LLVM load Instruction</seealso>
    public sealed class Load
        : UnaryInstruction
    {
        /// <summary>Gets or sets a value indicating whether this load is volatile</summary>
        public bool IsVolatile
        {
            get => LLVMGetVolatile( Handle );
            set => LLVMSetVolatile( Handle, value );
        }

        internal Load( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
