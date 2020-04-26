// -----------------------------------------------------------------------
// <copyright file="CatchSwitch.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Describes the set of possible catch handlers that may be executed by an
    /// <see href="xref:llvm_langref#personalityfn">EH personality routine</see></summary>
    /// <seealso href="xref:llvm_langref#i-catchswitch">LLVM catchswitch instruction</seealso>
    /// <seealso href="xref:llvm_exception_handling#exception-handling-in-llvm">Exception Handling in LLVM</seealso>
    /// <seealso href="xref:llvm_exception_handling#wineh">Exception Handling using the Windows Runtime</seealso>
    public class CatchSwitch
        : Instruction
    {
        /// <summary>Gets or sets the Parent pad for this <see cref="CatchSwitch"/></summary>
        public Value ParentPad
        {
            get => Operands.GetOperand<Value>( 0 )!;
            set => Operands[ 0 ] = value.ValidateNotNull( nameof(value) );
        }

        /* TODO: Enable UnwindDestination once the non-operand properties are enabled
            LLVMGetUnwindDest is available now...
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
            bool UnwindsToCaller => !HasUnwindDestination;
        */

        internal CatchSwitch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
