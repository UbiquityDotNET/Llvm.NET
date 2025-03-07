// -----------------------------------------------------------------------
// <copyright file="CatchSwitch.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.IRBindings;
using static Ubiquity.NET.Llvm.Interop.NativeMethods;

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
        [DisallowNull]
        public Value ParentPad
        {
            get => Operands.GetOperand<Value>( 0 )!;
            set => Operands[ 0 ] = value.ThrowIfNull();
        }

        /// <summary>Gets a value indicating whether this <see cref="CatchSwitch"/> has an unwind destination</summary>
        public bool HasUnwindDestination => LibLLVMHasUnwindDest( ValueHandle );

        /// <summary>Gets a value indicating whether this <see cref="CatchSwitch"/> unwinds to the caller</summary>
        public bool UnwindsToCaller => !HasUnwindDestination;

        /// <summary>Gets or sets the Unwind destination for this <see cref="CatchSwitch"/></summary>
        /// <remarks>
        /// While retrieving the destination may return null, setting with null will generate
        /// an exception. In particular if <see cref="HasUnwindDestination"/> is <see langword="false"/>
        /// then the UnwindDestination is <see langword="null"/>.
        /// </remarks>
        public BasicBlock? UnwindDestination
        {
            get
            {
                if( !HasUnwindDestination )
                {
                    return null;
                }

                var handle = LLVMGetUnwindDest( ValueHandle );
                return handle == default ? null : BasicBlock.FromHandle( handle );
            }

            set
            {
                ArgumentNullException.ThrowIfNull( value );
                if(!HasUnwindDestination)
                {
                    throw new InvalidOperationException( Resources.Cannot_set_unwindDestination_for_instruction_that_unwinds_to_caller );
                }

                LLVMSetUnwindDest( ValueHandle, value!.BlockHandle );
            }
        }

        internal CatchSwitch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
