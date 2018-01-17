// <copyright file="IndirectBranch.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to perform an indirect branch to a block within the current function</summary>
    /// <remarks>The address of the branch must come from a <see cref="BlockAddress"/> constant</remarks>
    /// <seealso href="xref:llvm_langref#indirectbr-instruction">LLVM indirectbr Instruction</seealso>
    public class IndirectBranch
        : Terminator
    {
        internal IndirectBranch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
