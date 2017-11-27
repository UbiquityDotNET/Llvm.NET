// <copyright file="CleanupPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Specifies that a <see cref="BasicBlock"/> is a cleanup block</summary>
    /// <seealso href="xref:llvm_langref#i-cleanuppad">LLVM 'cleanuppad' instruction</seealso>
    /// <seealso href="xref:llvm_exception_handling#exception-handling-in-llvm">Exception Handling in LLVM</seealso>
    /// <seealso href="xref:llvm_exception_handling#wineh">Exception Handling using the Windows Runtime</seealso>
    public class CleanupPad
        : FuncletPad
    {
        internal CleanupPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
