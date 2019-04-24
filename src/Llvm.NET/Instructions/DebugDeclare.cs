// <copyright file="DebugDeclare.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Intrinsic LLVM IR instruction to declare Debug information for a <see cref="Value"/></summary>
    /// <seealso href="xref:llvm_sourcelevel_debugging#llvm-dbg-declare">llvm.dbg.declare</seealso>
    /// <seealso href="xref:llvm_sourcelevel_debugging">LLVM Source Level Debugging</seealso>
    public class DebugDeclare
        : DebugInfoIntrinsic
    {
        internal DebugDeclare( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
