// -----------------------------------------------------------------------
// <copyright file="CleanupReturn.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.NET.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction that indicates to the personality function that one <see cref="CleanupPad"/> it transferred control to has ended</summary>
    /// <seealso href="xref:llvm_langref#cleanupret-instruction">LLVM cleanupret instruction</seealso>
    /// <seealso href="xref:llvm_exception_handling#exception-handling-in-llvm">Exception Handling in LLVM</seealso>
    /// <seealso href="xref:llvm_exception_handling#wineh">Exception Handling using the Windows Runtime</seealso>
    public class CleanupReturn
        : Terminator
    {
        /// <summary>Gets or sets the <see cref="CleanupPad"/> for this instruction</summary>
        public CleanupPad CleanupPad
        {
            get => Operands.GetOperand<CleanupPad>( 0 )!;
            set => Operands[ 0 ] = value.ValidateNotNull( nameof( value ) );
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
                value.ValidateNotNull( nameof( value ) );
                if( !HasUnwindDestination )
                {
                    throw new InvalidOperationException( Resources.Cannot_set_unwindDestination_for_instruction_that_unwinds_to_caller );
                }

                LLVMSetUnwindDest( ValueHandle, value!.BlockHandle );
            }
        }

        internal CleanupReturn( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
