// <copyright file="LandingPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET.Native;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using static Llvm.NET.Instructions.Instruction.NativeMethods;

namespace Llvm.NET.Instructions
{
    /// <summary>Marks a <see cref="BasicBlock"/> as a catch handler</summary>
    /// <remarks>
    /// Like the <see cref="CatchPad"/>, instruction this must be the first non-phi instruction
    /// in the block.
    /// </remarks>
    /// <seealso href="xref:llvm_langref#i-landingpad">LLVM landing Instruction</seealso>
    /// <seealso href="xref:llvm_exception_handling#exception-handling-in-llvm">Exception Handling in LLVM</seealso>
    public class LandingPad
        : Instruction
        , IOperandContainer<Constant>
    {
        /// <summary>Gets the list of clauses for this pad</summary>
        public IList<Constant> Clauses { get; }

        /// <summary>Gets or sets a value indicating whether this <see cref="LandingPad"/> is a cleanup pad</summary>
        public bool IsCleanup
        {
            get => LLVMIsCleanup( ValueHandle );
            set => LLVMSetCleanup( ValueHandle, value );
        }

        /// <inheritdoc/>
        long IOperandContainer<Constant>.Count => Operands.Count;

        /// <inheritdoc/>
        Constant IOperandContainer<Constant>.this[ int index ]
        {
            get => GetOperand<Constant>( index );
            set => Operands[ index ] = value;
        }

        /// <inheritdoc/>
        void IOperandContainer<Constant>.Add( Constant item )
        {
            item.ValidateNotNull( nameof( item ) );

            LLVMAddClause( ValueHandle, item.ValueHandle );
        }

        internal LandingPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Clauses = new OperandList<Constant>( this );
        }
    }
}
