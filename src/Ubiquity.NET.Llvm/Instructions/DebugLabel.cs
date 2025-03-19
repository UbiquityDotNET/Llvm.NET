// -----------------------------------------------------------------------
// <copyright file="DebugDeclare.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Intrinsic LLVM IR instruction to provide a Debug label for a <see cref="Value"/></summary>
    /// <seealso href="xref:llvm_sourcelevel_debugging#llvm-dbg-label">llvm.dbg.declare</seealso>
    /// <seealso href="xref:llvm_sourcelevel_debugging">LLVM Source Level Debugging</seealso>
    public sealed class DebugLabel
        : DebugInfoIntrinsic
    {
        internal DebugLabel( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
