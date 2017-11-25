// <copyright file="CleanupReturn.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction that indicates to the personality function that one <see cref="CleanupPad"/> it transferred control to has ended</summary>
    /// <seealso href="xref:llvm_langref#cleanupret-instruction">LLVM 'cleanupret' instruction</seealso>
    /// <seealso href="xref:llvm_releases_docs/ExceptionHandling.html">Exception Handling in LLVM</seealso>
    /// <seealso href="xref:llvm_releases_docs/ExceptionHandle.html#wineh">Exception Handling using the Windows Runtime</seealso>
    public class CleanupReturn
        : Terminator
    {
        /// <summary>Gets the <see cref="CleanupPad"/> for this instruction</summary>
        public CleanupPad CleanupPad => GetOperand<CleanupPad>( 0 );
        /* TODO: CleanupPad { set; } */

        /* TODO:
            BasicBlock UnwindDestination
            {
                get => HasUnwindDestination ? GetOperand<BasicBlock>( 1 );
                set
                {
                    if(!HasUnwindDestination)
                    {
                        throw new ArgumentException();
                    }
                    SetOperand(1, value);
                }
            }
        */

        /* TODO: non-operand properties
            bool HasUnwindDestination { get; }
            bool UnwindsToCaller { get; }
        */
        internal CleanupReturn( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
