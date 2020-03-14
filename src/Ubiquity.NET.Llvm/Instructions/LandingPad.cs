// -----------------------------------------------------------------------
// <copyright file="LandingPad.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
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
        Constant? IOperandContainer<Constant>.this[ int index ]
        {
            get => GetOperand<Constant>( index );
            set => Operands[ index ] = value.ValidateNotNull( nameof( value ) )!;
        }

        /// <inheritdoc/>
        /// <remarks>The <paramref name="item"/> parameter must not be <see langword="null"/></remarks>
        void IOperandContainer<Constant>.Add( Constant? item )
        {
            item.ValidateNotNull( nameof( item ) );

            LLVMAddClause( ValueHandle, item!.ValueHandle );
        }

        internal LandingPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Clauses = new OperandList<Constant>( this );
        }
    }
}
