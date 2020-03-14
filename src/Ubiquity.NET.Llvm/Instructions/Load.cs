// -----------------------------------------------------------------------
// <copyright file="Load.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
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
