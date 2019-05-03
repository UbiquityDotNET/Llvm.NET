// <copyright file="Alloca.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Alloca instruction for allocating stack space</summary>
    /// <remarks>
    /// LLVM Mem2Reg pass will convert alloca locations to register for the
    /// entry block to the maximum extent possible.
    /// </remarks>
    /// <seealso href="xref:llvm_langref#alloca-instruction">LLVM alloca</seealso>
    public class Alloca
        : UnaryInstruction
    {
        internal Alloca( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
