// <copyright file="Load.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

using static Llvm.NET.Instructions.Instruction.NativeMethods;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to read from memory</summary>
    /// <seealso href="xref:llvm_langref#load-instruction">LLVM load Instruction</seealso>
    public class Load
        : UnaryInstruction
    {
        /// <summary>Gets or sets a value indicating whether this load is volatile</summary>
        public bool IsVolatile
        {
            get => LLVMGetVolatile( ValueHandle );
            set => LLVMSetVolatile( ValueHandle, value );
        }

        internal Load( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
