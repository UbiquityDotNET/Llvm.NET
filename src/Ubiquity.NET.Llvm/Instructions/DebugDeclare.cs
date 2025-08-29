// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Intrinsic LLVM IR instruction to declare Debug information for a <see cref="Value"/></summary>
    /// <seealso href="xref:llvm_sourcelevel_debugging#llvm-dbg-declare">llvm.dbg.declare</seealso>
    /// <seealso href="xref:llvm_sourcelevel_debugging">LLVM Source Level Debugging</seealso>
    public sealed class DebugDeclare
        : DebugInfoIntrinsic
    {
        internal DebugDeclare( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
