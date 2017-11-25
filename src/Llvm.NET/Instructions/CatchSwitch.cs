// <copyright file="CatchSwitch.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Describes the set of possible catch handlers that may be executed by an
    /// <see href="xref:llvm_langref#personalityfn">EH personality routine</see></summary>
    /// <seealso href="xref:llvm_langref#i-catchswitch">LLVM 'catchswitch' instruction</seealso>
    /// <seealso href="xref:llvm_releases_docs/ExceptionHandling.html">Exception Handling in LLVM</seealso>
    /// <seealso href="xref:llvm_releases_docs/ExceptionHandle.html#wineh">Exception Handling using the Windows Runtime</seealso>
    public class CatchSwitch
        : Instruction
    {
        /// <summary>Gets the Parent pad for this <see cref="CatchSwitch"/></summary>
        public Value ParentPad => GetOperand<Value>( 0 );
        /* TODO: ParentPad { set; } */

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

        internal CatchSwitch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
